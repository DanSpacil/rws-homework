using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Persistence;
using TranslationManagement.Api.Translators;

namespace TranslationManagement.Api.Jobs;

[ApiController]
[Route("api/jobs/[action]")]
public class TranslationJobController : ControllerBase
{
    private AppDbContext _context;
    private readonly ILogger<TranslatorManagementController> _logger;
    private readonly ITranslationJobService _translationJobService;
    private readonly TranslationJobMapper _translationJobMapper;

    public TranslationJobController(IServiceScopeFactory scopeFactory, ILogger<TranslatorManagementController> logger, ITranslationJobService translationJobService, TranslationJobMapper translationJobMapper)
    {
        _context = scopeFactory.CreateScope().ServiceProvider.GetService<AppDbContext>();
        _logger = logger;
        _translationJobService = translationJobService;
        _translationJobMapper = translationJobMapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetJobs()
    {
        var jobs = await _translationJobService.GetAll();
        return Ok(_translationJobMapper.Map(jobs));
    }

    const double PricePerCharacter = 0.01;

    private void SetPrice(TranslationJob job)
    {
        job.Price = job.OriginalContent.Length * PricePerCharacter;
    }

    [HttpPost]
    public bool CreateJob(TranslationJob job)
    {
        job.Status = "New";
        SetPrice(job);
        _context.TranslationJobs.Add(job);
        var success = _context.SaveChanges() > 0;
        if (success)
        {
            var notificationSvc = new UnreliableNotificationService();
            while (!notificationSvc.SendNotification("Job created: " + job.Id).Result)
            {
            }

            _logger.LogInformation("New job notification sent");
        }

        return success;
    }

    [HttpPost]
    public bool CreateJobWithFile(IFormFile file, string customer)
    {
        var reader = new StreamReader(file.OpenReadStream());
        string content;

        if (file.FileName.EndsWith(".txt"))
        {
            content = reader.ReadToEnd();
        }
        else if (file.FileName.EndsWith(".xml"))
        {
            var xdoc = XDocument.Parse(reader.ReadToEnd());
            content = xdoc.Root.Element("Content").Value;
            customer = xdoc.Root.Element("Customer").Value.Trim();
        }
        else
        {
            throw new NotSupportedException("unsupported file");
        }

        var newJob = new TranslationJob() { OriginalContent = content, TranslatedContent = "", CustomerName = customer, };

        SetPrice(newJob);

        return CreateJob(newJob);
    }

    [HttpPost]
    public string UpdateJobStatus(int jobId, int translatorId, string newStatus = "")
    {
        _logger.LogInformation("Job status update request received: " + newStatus + " for job " + jobId.ToString() + " by translator " + translatorId);
        if (typeof(JobStatuses).GetProperties().Count(prop => prop.Name == newStatus) == 0)
        {
            return "invalid status";
        }

        var job = _context.TranslationJobs.Single(j => j.Id == jobId);

        var isInvalidStatusChange = (job.Status == JobStatuses.New && newStatus == JobStatuses.Completed) ||
                                    job.Status == JobStatuses.Completed || newStatus == JobStatuses.New;
        if (isInvalidStatusChange)
        {
            return "invalid status change";
        }

        job.Status = newStatus;
        _context.SaveChanges();
        return "updated";
    }
}
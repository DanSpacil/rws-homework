using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Translators;

namespace TranslationManagement.Api.Jobs;

[ApiController]
[Route("api/jobs/[action]")]
public class TranslationJobController : ControllerBase
{
    private readonly ILogger<TranslatorManagementController> _logger;
    private readonly ITranslationJobService _translationJobService;
    private readonly TranslationJobMapper _translationJobMapper;

    public TranslationJobController(ILogger<TranslatorManagementController> logger, ITranslationJobService translationJobService, TranslationJobMapper translationJobMapper)
    {
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

    [HttpPost]
    public async Task<OkObjectResult> CreateJob(CreateJobRequest createJobRequest)
    {
        var createJobResult = await _translationJobService.CreateJob(new CreateJobCommand(createJobRequest.CustomerName, createJobRequest.OriginalContent));
        if (createJobResult.IsSuccess)
        {
            var notificationSvc = new UnreliableNotificationService();
            while (!notificationSvc.SendNotification("Job created: " + createJobResult.JobId).Result)
            {
            }

            _logger.LogInformation("New job notification sent");
        }

        return Ok(createJobResult.IsSuccess);
    }

    [HttpPost]
    public async Task<OkObjectResult> CreateJobWithFile(IFormFile file, string customer)
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

        var createJobResult = await _translationJobService.CreateJob(new CreateJobCommand(customer, content));

        return Ok(createJobResult.IsSuccess);
    }

    [HttpPost]
    public async Task<string> UpdateJobStatus(int jobId, int translatorId, JobStatus newStatus)
    {
        _logger.LogInformation("Job status update request received: " + newStatus + " for job " + jobId.ToString() + " by translator " + translatorId);
        // if (typeof(JobStatuses).GetProperties().Count(prop => prop.Name == newStatus) == 0)
        // {
        //     return "invalid status";
        // }
        var updateResult = await this._translationJobService.UpdateJobStatus(new JobStatusUpdateCommand(jobId, newStatus));
        return updateResult.IsUpdated ? "update" : "invalid status";
    }
}

public class CreateJobRequest
{
    public string CustomerName { get; set; }
    public string OriginalContent { get; set; }
}
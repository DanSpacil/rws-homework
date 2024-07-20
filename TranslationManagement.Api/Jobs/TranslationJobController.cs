using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.FileParsing;
using TranslationManagement.Api.Http;
using TranslationManagement.Api.Translators;

namespace TranslationManagement.Api.Jobs;

public class TranslationJobController : ApiController
{
    private readonly ILogger<TranslatorManagementController> _logger;
    private readonly ITranslationJobService _translationJobService;
    private readonly TranslationJobMapper _translationJobMapper;
    private readonly IFileParsingProvider _jobParsingProvider;

    public TranslationJobController(ILogger<TranslatorManagementController> logger, ITranslationJobService translationJobService, TranslationJobMapper translationJobMapper, IFileParsingProvider jobParsingProvider)
    {
        _logger = logger;
        _translationJobService = translationJobService;
        _translationJobMapper = translationJobMapper;
        _jobParsingProvider = jobParsingProvider;
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
        return Ok(createJobResult.IsSuccess);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJobWithFile(IFormFile file, string customer)
    {
        var createJobRequest = _jobParsingProvider.Parse(file, customer);
        if (!createJobRequest.IsSuccess)
        {
            return BadRequest(createJobRequest.FailReason);
        }
        var createJobResult = await _translationJobService.CreateJob(new CreateJobCommand(createJobRequest.CustomerName, createJobRequest.OriginalContent));
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
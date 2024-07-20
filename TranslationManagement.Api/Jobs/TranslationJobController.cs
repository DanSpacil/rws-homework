using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Http;
using TranslationManagement.Api.Translators;

namespace TranslationManagement.Api.Jobs;

public class TranslationJobController : ApiController
{
    private readonly ILogger<TranslatorController> _logger;
    private readonly ITranslationJobService _translationJobService;
    private readonly TranslationJobMapper _translationJobMapper;

    public TranslationJobController(ILogger<TranslatorController> logger, ITranslationJobService translationJobService,
        TranslationJobMapper translationJobMapper)
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
    public async Task<IActionResult> CreateJob(CreateJobRequest createJobRequest)
    {
        var createJobResult =
            await _translationJobService.CreateJob(new CreateJobCommand(createJobRequest.CustomerName,
                createJobRequest.OriginalContent));
        return Ok(createJobResult.IsSuccess);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJobWithFile(IFormFile file, string customer)
    {
        var createJobResult = await _translationJobService.CreateJobFromFile(file, customer);
        return Ok(createJobResult.IsSuccess);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateJobStatus(int jobId, int translatorId, JobStatus newStatus)
    {
        _logger.LogInformation(
            "Job status update request received: {NewJobStatus} for job {JobId} by translator {TranslatorId}",
            newStatus, jobId, translatorId);
        var updateResult = await _translationJobService.UpdateJobStatus(new JobStatusUpdateCommand(jobId, newStatus));
        return updateResult.IsSuccess ? Ok("update") : BadRequest("invalid status");
    }
}
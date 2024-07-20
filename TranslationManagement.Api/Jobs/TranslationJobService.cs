using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TranslationManagement.Api.FileParsing;
using TranslationManagement.Api.Jobs.Persistence;
using TranslationManagement.Api.Notifications;
using TranslationManagement.Api.Workflow;

namespace TranslationManagement.Api.Jobs;

public class TranslationJobService : ITranslationJobService
{
    private readonly ITranslationJobRepository _translationJobRepository;
    private readonly INotifier _notifier;
    private readonly IFileParsingProvider _jobParsingProvider;

    private const double PricePerCharacter = 0.01;

    public TranslationJobService(INotifier notifier, ITranslationJobRepository translationJobRepository,
        IFileParsingProvider jobParsingProvider)
    {
        _notifier = notifier;
        _translationJobRepository = translationJobRepository;
        _jobParsingProvider = jobParsingProvider;
    }

    public async Task<ICollection<TranslationJob>> GetAll()
    {
        return await _translationJobRepository.GetAllJobs();
    }

    public async Task<Result<JobCreated>> CreateJob(CreateJobCommand createJobCommand)
    {
        var job = new TranslationJob
        {
            CustomerName = createJobCommand.CustomerName,
            Status = JobStatus.New,
            OriginalContent = createJobCommand.OriginalContent
        };
        job.SetPrice(PricePerCharacter);
        var entity = await _translationJobRepository.AddJob(job);
        var saveResult = await _translationJobRepository.SaveChangesAsync();
        if (saveResult == 0)
        {
            return Result<JobCreated>.Error("Unexpected issue when saving job");
        }

        await _notifier.Notify(new JobCreatedNotification(job.Id));
        return Result<JobCreated>.Success(new JobCreated(entity.Id));
    }

    public async Task<Result<bool>> UpdateJobStatus(JobStatusUpdateCommand jobStatusUpdateCommand)
    {
        var job = await _translationJobRepository.GetJobById(jobStatusUpdateCommand.JobId);
        if (job is null)
        {
            return Result<bool>.Error("Job for given id not found");
        }

        var updateResult = job.UpdateStatus(jobStatusUpdateCommand.NewStatus);
        if (updateResult.IsSuccess)
        {
            await _translationJobRepository.SaveChangesAsync();
        }

        return updateResult;
    }

    public async Task<Result<JobCreated>> CreateJobFromFile(IFormFile file, string customer)
    {
        var createJobRequest = _jobParsingProvider.Parse(file, customer);
        if (!createJobRequest.IsSuccess)
        {
            return Result<JobCreated>.Error(createJobRequest.FailReason);
        }

        return await CreateJob(new CreateJobCommand(createJobRequest.Data.CustomerName,
            createJobRequest.Data.OriginalContent));
    }
}
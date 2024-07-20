using System.Collections.Generic;
using System.Threading.Tasks;
using TranslationManagement.Api.Jobs.Persistence;
using TranslationManagement.Api.Notifications;

namespace TranslationManagement.Api.Jobs;

public class TranslationJobService : ITranslationJobService
{
    private readonly ITranslationJobRepository _translationJobRepository;
    private readonly INotifier _notifier;

    private const double PricePerCharacter = 0.01;

    public TranslationJobService(INotifier notifier, ITranslationJobRepository translationJobRepository)
    {
        _notifier = notifier;
        _translationJobRepository = translationJobRepository;
    }

    public async Task<IEnumerable<TranslationJob>> GetAll()
    {
        return await _translationJobRepository.GetAllJobs();
    }

    public async Task<JobCreatedResult> CreateJob(CreateJobCommand createJobCommand)
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
            return JobCreatedResult.Error();
        }

        await _notifier.Notify(new JobCreatedNotification(job.Id));
        return JobCreatedResult.Success(entity.Id);
    }

    public async Task<JobStatusUpdateResult> UpdateJobStatus(JobStatusUpdateCommand jobStatusUpdateCommand)
    {
        var job = await _translationJobRepository.GetJobById(jobStatusUpdateCommand.JobId); 
        if (job is null)
        {
            return JobStatusUpdateResult.Invalid();
        }

        var updateResult = job.UpdateStatus(jobStatusUpdateCommand.NewStatus);
        if (updateResult.IsUpdated)
        {
            await _translationJobRepository.SaveChangesAsync();
        }

        return updateResult;
    }
}
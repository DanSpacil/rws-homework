using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Notifications;
using TranslationManagement.Api.Persistence;

namespace TranslationManagement.Api.Jobs;

public class TranslationJobService : ITranslationJobService
{
    private readonly AppDbContext _appContext;
    private readonly INotifier _notifier;

    private const double PricePerCharacter = 0.01;

    public TranslationJobService(AppDbContext appContext, INotifier notifier)
    {
        _appContext = appContext;
        _notifier = notifier;
    }

    public async Task<IEnumerable<TranslationJob>> GetAll()
    {
        return await _appContext.TranslationJobs.ToListAsync();
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
        var entity = await _appContext.TranslationJobs.AddAsync(job);
        var saveResult = await _appContext.SaveChangesAsync();
        if (saveResult == 0)
        {
            return JobCreatedResult.Error();
        }

        await _notifier.Notify(new JobCreatedNotification(job.Id));
        return JobCreatedResult.Success(entity.Entity.Id);
    }

    public async Task<JobStatusUpdateResult> UpdateJobStatus(JobStatusUpdateCommand jobStatusUpdateCommand)
    {
        var job = await _appContext.TranslationJobs
            .FirstOrDefaultAsync(j => j.Id == jobStatusUpdateCommand.JobId);
        if (job is null)
        {
            return JobStatusUpdateResult.Invalid();
        }

        var updateResult = job.UpdateStatus(jobStatusUpdateCommand.NewStatus);
        if (updateResult.IsUpdated)
        {
            await _appContext.SaveChangesAsync();
        }

        return updateResult;
    }
}
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Persistence;

namespace TranslationManagement.Api.Jobs;

public class TranslationJobService : ITranslationJobService
{
    private readonly AppDbContext _appContext;
    
    const double PricePerCharacter = 0.01;

    public TranslationJobService(AppDbContext appContext)
    {
        _appContext = appContext;
    }

    public async Task<IEnumerable<TranslationJob>> GetAll()
    {
        return await _appContext.TranslationJobs.ToListAsync();
    }

    public async Task<JobCreatedResult> CreateJob(CreateJobCommand createJobCommand)
    {
        var job = new TranslationJob { CustomerName = createJobCommand.CustomerName, Status = JobStatus.New, OriginalContent = createJobCommand.OriginalContent };
        job.SetPrice(PricePerCharacter);
        var entity = await _appContext.TranslationJobs.AddAsync(job);
        var saveResult = await _appContext.SaveChangesAsync();
        return saveResult > 0
            ? JobCreatedResult.Success(entity.Entity.Id)
            : JobCreatedResult.Error();
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

public record JobStatusUpdateCommand(int JobId, JobStatus NewStatus);

public class JobCreatedResult
{
    private JobCreatedResult() {}
    
    [MemberNotNullWhen(true, nameof(IsSuccess))]
    public int? JobId { get; private init; }

    public bool IsSuccess { get; private init; }

    public static JobCreatedResult Success(int jobId)
        => new () { IsSuccess = true, JobId = jobId };

    public static JobCreatedResult Error()
        => new () { IsSuccess = true };
}

public record CreateJobCommand(string CustomerName, string OriginalContent);
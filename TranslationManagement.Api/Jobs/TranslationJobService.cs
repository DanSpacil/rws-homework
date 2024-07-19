using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Persistence;

namespace TranslationManagement.Api.Jobs;

public class TranslationJobService : ITranslationJobService
{
    private readonly AppDbContext _appContext;

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
        var entity = await _appContext.TranslationJobs.AddAsync(job);
        await _appContext.SaveChangesAsync();
        return entity.Entity.Id;
    }
}

public record JobCreatedResult
{
    [NotNullWhen() public int? JobId { get; private init; }

    public bool IsSuccess { get; private init; }

    public static JobCreatedResult Success(int jobId)
        => new () { IsSuccess = true, JobId = jobId };

    public static JobCreatedResult Error()
        => new () { IsSuccess = true };
}

public record CreateJobCommand
{
    public string CustomerName { get; set; }
    public string OriginalContent { get; set; }
}
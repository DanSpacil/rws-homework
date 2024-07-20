using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Persistence;

namespace TranslationManagement.Api.Jobs.Persistence;

public class TranslationJobRepository : ITranslationJobRepository
{
    private readonly AppDbContext _appContext;

    public TranslationJobRepository(AppDbContext appContext)
    {
        _appContext = appContext;
    }

    public async Task<TranslationJob> GetJobById(int jobId)
    {
        return await _appContext.TranslationJobs.FirstOrDefaultAsync(j => j.Id == jobId);
    }

    public async Task<ICollection<TranslationJob>> GetAllJobs()
    {
        return await _appContext.TranslationJobs.ToListAsync();
    }

    public async Task<TranslationJob> AddJob(TranslationJob job)
    {
        var savedJob = await _appContext.TranslationJobs.AddAsync(job);
        return savedJob.Entity;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _appContext.SaveChangesAsync();
    }
}
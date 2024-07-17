using System.Collections.Generic;
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
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Persistence;

namespace TranslationManagement.Api.Translators;

public class TranslatorService : ITranslatorService
{
    private readonly AppDbContext _appContext;

    public TranslatorService(AppDbContext appContext)
    {
        _appContext = appContext;
    }

    public async Task<ICollection<TranslatorModel>> GetAllTranslators()
    {
        return await _appContext.Translators.ToListAsync();
    }

    public async Task<ICollection<TranslatorModel>> GetByName(string name)
    {
        return await _appContext.Translators.Where(t => t.Name == name).ToListAsync();
    }

    public async Task<bool> AddTranslator(TranslatorModel translator)
    {
        await _appContext.AddAsync(translator);
        return await _appContext.SaveChangesAsync() > 0;
    }

    public async Task UpdateTranslatorStatus(int translatorId, TranslatorStatus newStatus)
    {
        var job = _appContext.Translators.Single(j => j.Id == translatorId);
        job.Status = newStatus;
        await _appContext.SaveChangesAsync();
    }
}
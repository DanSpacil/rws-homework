using System;
using System.Collections.Generic;
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
}
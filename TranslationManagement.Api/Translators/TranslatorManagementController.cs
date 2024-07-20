using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Http;
using TranslationManagement.Api.Persistence;

namespace TranslationManagement.Api.Translators;

public class TranslatorManagementController : ApiController
{
    public static readonly string[] TranslatorStatuses = { "Applicant", "Certified", "Deleted" };

    private readonly ILogger<TranslatorManagementController> _logger;

    private readonly ITranslatorService _translatorService;
    private readonly TranslatorMapper _translatorMapper;

    public TranslatorManagementController(ILogger<TranslatorManagementController> logger, ITranslatorService translatorService, TranslatorMapper translatorMapper)
    {
        _logger = logger;
        _translatorService = translatorService;
        _translatorMapper = translatorMapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetTranslators()
    {
        var translators = await _translatorService.GetAllTranslators();
        return this.Ok(_translatorMapper.Map(translators));
    }

    [HttpGet]
    public async Task<IActionResult> GetTranslatorsByName(string name)
    {
        var translators = await _translatorService.GetByName(name);
        return this.Ok(_translatorMapper.Map(translators));
    }

    [HttpPost]
    public async Task<bool> AddTranslator(TranslatorModel translator)
    {
        var translatorAdded = await _translatorService.AddTranslator(translator);
        return translatorAdded;
    }

    [HttpPost]
    public async Task<string> UpdateTranslatorStatus(int Translator, string newStatus = "")
    {
        _logger.LogInformation("User status update request: " + newStatus + " for user " + Translator.ToString());
        if (TranslatorStatuses.Where(status => status == newStatus).Count() == 0)
        {
            throw new ArgumentException("unknown status");
        }

        await _translatorService.UpdateTranslatorStatus(Translator, newStatus);
        return "updated";
    }
}
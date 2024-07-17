using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Persistence;

namespace TranslationManagement.Api.Translators;

[ApiController]
[Route("api/TranslatorsManagement/[action]")]
public class TranslatorManagementController : ControllerBase
{
    public static readonly string[] TranslatorStatuses = { "Applicant", "Certified", "Deleted" };

    private readonly ILogger<TranslatorManagementController> _logger;
    private AppDbContext _context;

    private readonly ITranslatorService _translatorService;
    private readonly TranslatorMapper _translatorMapper;

    public TranslatorManagementController(IServiceScopeFactory scopeFactory, ILogger<TranslatorManagementController> logger, ITranslatorService translatorService, TranslatorMapper translatorMapper)
    {
        _context = scopeFactory.CreateScope().ServiceProvider.GetService<AppDbContext>();
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
    public bool AddTranslator(TranslatorModel translator)
    {
        _context.Translators.Add(translator);
        return _context.SaveChanges() > 0;
    }

    [HttpPost]
    public string UpdateTranslatorStatus(int Translator, string newStatus = "")
    {
        _logger.LogInformation("User status update request: " + newStatus + " for user " + Translator.ToString());
        if (TranslatorStatuses.Where(status => status == newStatus).Count() == 0)
        {
            throw new ArgumentException("unknown status");
        }

        var job = _context.Translators.Single(j => j.Id == Translator);
        job.Status = newStatus;
        _context.SaveChanges();

        return "updated";
    }
}
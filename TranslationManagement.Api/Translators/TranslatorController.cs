using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Http;

namespace TranslationManagement.Api.Translators;

public class TranslatorController : ApiController
{

    private readonly ILogger<TranslatorController> _logger;
    private readonly ITranslatorService _translatorService;
    private readonly TranslatorMapper _translatorMapper;

    public TranslatorController(ILogger<TranslatorController> logger, ITranslatorService translatorService, TranslatorMapper translatorMapper)
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
    public async Task<IActionResult> AddTranslator(TranslatorModel translator)
    {
        var translatorAdded = await _translatorService.AddTranslator(translator);
        return Ok(translatorAdded);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTranslatorStatus(int translatorId, TranslatorStatus newStatus)
    {
        _logger.LogInformation("User status update request: {NewTranslatorStatus} for user {TranslatorId}", newStatus, translatorId);
        await _translatorService.UpdateTranslatorStatus(translatorId, newStatus);
        return Ok("updated");
    }
}
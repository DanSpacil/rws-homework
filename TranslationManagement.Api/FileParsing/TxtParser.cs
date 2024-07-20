using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TranslationManagement.Api.FileParsing;

public class TxtParser : IJobParser
{
    public string SupportedFileType => "txt";
    private readonly ILogger<TxtParser> _logger;

    public TxtParser(ILogger<TxtParser> logger)
    {
        _logger = logger;
    }

    public ParseJobResult Parse(IFormFile file, string customer)
    {
        try
        {
            using var streamReader = new StreamReader(file.OpenReadStream());
            return ParseJobResult.Success(customer, streamReader.ReadToEnd());
        }
        catch (Exception _)
        {
            _logger.LogTrace("Exception during XML parsing");
            return ParseJobResult.Error("Unable to process XML content");
        }
    }
}
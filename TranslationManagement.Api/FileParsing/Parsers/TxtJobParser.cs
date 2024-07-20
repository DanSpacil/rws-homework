using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TranslationManagement.Api.FileParsing.Parsers;

public class TxtJobParser : IJobParser
{
    public string SupportedFileType => "txt";
    private readonly ILogger<TxtJobParser> _logger;

    public TxtJobParser(ILogger<TxtJobParser> logger)
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
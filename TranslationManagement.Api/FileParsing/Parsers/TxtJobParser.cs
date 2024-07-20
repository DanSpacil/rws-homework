using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Jobs;
using TranslationManagement.Api.Workflow;

namespace TranslationManagement.Api.FileParsing.Parsers;

public class TxtJobParser : IJobParser
{
    public string SupportedFileType => "txt";
    private readonly ILogger<TxtJobParser> _logger;

    public TxtJobParser(ILogger<TxtJobParser> logger)
    {
        _logger = logger;
    }

    public Result<CreateJobRequest> Parse(IFormFile file, string customer)
    {
        try
        {
            using var streamReader = new StreamReader(file.OpenReadStream());
            var request = new CreateJobRequest { CustomerName = customer, OriginalContent = streamReader.ReadToEnd() };
            return Result<CreateJobRequest>.Success(request);
        }
        catch (Exception _)
        {
            _logger.LogTrace("Exception during XML parsing");
            return Result<CreateJobRequest>.Error("Unable to process XML content");
        }
    }
}
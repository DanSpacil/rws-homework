using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Jobs;
using TranslationManagement.Api.Workflow;

namespace TranslationManagement.Api.FileParsing.Parsers;

public class XmlJobParser : IJobParser
{
    private readonly ILogger<XmlJobParser> _logger;
    public string SupportedFileType => "xml";

    public XmlJobParser(ILogger<XmlJobParser> logger)
    {
        _logger = logger;
    }


    public Result<CreateJobRequest> Parse(IFormFile file, string customer)
    {
        try
        {
            using var streamReader = new StreamReader(file.OpenReadStream());
            var xdoc = XDocument.Parse(streamReader.ReadToEnd());
            var request = new CreateJobRequest
            {
                CustomerName = xdoc.Root.Element("Content").Value,
                OriginalContent = xdoc.Root.Element("Customer").Value.Trim()
            };
            return Result<CreateJobRequest>.Success(request);
        }
        catch (Exception _)
        {
            _logger.LogTrace("Exception during XML parsing");
            return Result<CreateJobRequest>.Error("Unable to process XML content");
        }
    }
}
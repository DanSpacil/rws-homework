using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TranslationManagement.Api.FileParsing;

public class XmlJobParser : IJobParser
{
    private readonly ILogger<XmlJobParser> _logger;
    public string SupportedFileType => "xml";

    public XmlJobParser(ILogger<XmlJobParser> logger)
    {
        _logger = logger;
    }


    public ParseJobResult Parse(IFormFile file, string customer)
    {
        try
        {
            using var streamReader = new StreamReader(file.OpenReadStream());
            var xdoc = XDocument.Parse(streamReader.ReadToEnd());
            var content = xdoc.Root.Element("Content").Value;
            var customerName = xdoc.Root.Element("Customer").Value.Trim();
            return ParseJobResult.Success(customerName, content);
        }
        catch (Exception _)
        {
            _logger.LogTrace("Exception during XML parsing");
            return ParseJobResult.Error("Unable to process XML content");
        }
    }

    }
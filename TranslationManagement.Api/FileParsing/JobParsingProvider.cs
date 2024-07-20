using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using TranslationManagement.Api.FileParsing.Parsers;

namespace TranslationManagement.Api.FileParsing;

public class JobParsingProvider : IFileParsingProvider
{
    private readonly Dictionary<string, IJobParser> _parsers;

    public JobParsingProvider(Dictionary<string, IJobParser> parsers)
    {
        _parsers = parsers;
    }

    public ParseJobResult Parse(IFormFile file, string customer)
    {
        var sanitizedFileExtension = Path.GetExtension(file.FileName).ToLower().Trim('.');
        var canParse = _parsers.TryGetValue(sanitizedFileExtension, out var parser);
        if (!canParse)
        {
            return ParseJobResult.Error("Unable to parse file");
        }

        return parser.Parse(file, customer);
    }
}
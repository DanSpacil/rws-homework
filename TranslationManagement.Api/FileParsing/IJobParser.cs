using Microsoft.AspNetCore.Http;

namespace TranslationManagement.Api.FileParsing;

public interface IJobParser
{
    string SupportedFileType { get; }
    ParseJobResult Parse(IFormFile file, string customer);
}
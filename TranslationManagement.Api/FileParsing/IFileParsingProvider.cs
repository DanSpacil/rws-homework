using Microsoft.AspNetCore.Http;

namespace TranslationManagement.Api.FileParsing;

public interface IFileParsingProvider
{
    ParseJobResult Parse(IFormFile file, string customer);
}
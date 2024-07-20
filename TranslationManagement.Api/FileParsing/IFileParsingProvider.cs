using Microsoft.AspNetCore.Http;
using TranslationManagement.Api.Jobs;
using TranslationManagement.Api.Workflow;

namespace TranslationManagement.Api.FileParsing;

public interface IFileParsingProvider
{
    Result<CreateJobRequest> Parse(IFormFile file, string customer);
}
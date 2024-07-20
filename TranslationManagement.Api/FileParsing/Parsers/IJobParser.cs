using Microsoft.AspNetCore.Http;
using TranslationManagement.Api.Jobs;
using TranslationManagement.Api.Workflow;

namespace TranslationManagement.Api.FileParsing.Parsers;

public interface IJobParser
{
    string SupportedFileType { get; }
    
    Result<CreateJobRequest> Parse(IFormFile file, string customer);
}
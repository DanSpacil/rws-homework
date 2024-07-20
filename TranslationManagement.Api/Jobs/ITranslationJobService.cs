using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TranslationManagement.Api.Workflow;

namespace TranslationManagement.Api.Jobs;

public interface ITranslationJobService
{
    Task<ICollection<TranslationJob>> GetAll();
    Task<Result<JobCreated>> CreateJob(CreateJobCommand createJobCommand);
    Task<Result<bool>> UpdateJobStatus(JobStatusUpdateCommand jobStatusUpdateCommand);
    Task<Result<JobCreated>> CreateJobFromFile(IFormFile file, string customer);
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TranslationManagement.Api.Jobs.Persistence;

public interface ITranslationJobRepository
{
    Task<TranslationJob?> GetJobById(int jobId);
    Task<ICollection<TranslationJob>> GetAllJobs();
    Task<TranslationJob> AddJob(TranslationJob job);
    Task<int> SaveChangesAsync();
}
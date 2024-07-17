using System.Collections.Generic;
using System.Threading.Tasks;

namespace TranslationManagement.Api.Jobs;

public interface ITranslationJobService
{
    Task<IEnumerable<TranslationJob>> GetAll();
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TranslationManagement.Api.Translators;

public interface ITranslatorService
{
    Task<ICollection<TranslatorModel>> GetAllTranslators();

    Task<ICollection<TranslatorModel>> GetByName(string name);
}
using System.Collections.Generic;
using System.Linq;

namespace TranslationManagement.Api.Jobs;

public class TranslationJobMapper
{
    public IEnumerable<TranslationJobGetModel> Map(IEnumerable<TranslationJob> translationJobs)
    {
        return translationJobs.Select(j => Map(j));
    }

    public TranslationJobGetModel Map(TranslationJob translationJob)
    {
        return new TranslationJobGetModel
        {
            Id = translationJob.Id,
            CustomerName = translationJob.CustomerName,
            Status = translationJob.Status,
            OriginalContent = translationJob.OriginalContent,
            TranslatedContent = translationJob.TranslatedContent,
            Price = translationJob.Price
        };
    }
}
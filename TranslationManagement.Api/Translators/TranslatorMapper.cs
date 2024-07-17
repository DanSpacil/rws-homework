using System.Collections.Generic;
using System.Linq;

namespace TranslationManagement.Api.Translators;

public class TranslatorMapper
{
    public IEnumerable<TranslatorGetModel> Map(IEnumerable<TranslatorModel> translators)
    {
        return translators.Select(t => Map((TranslatorModel)t));
    }

    public TranslatorGetModel Map(TranslatorModel translator)
    {
        return new TranslatorGetModel
        {
            Id = translator.Id,
            Name = translator.Name,
            HourlyRate = translator.HourlyRate,
            Status = translator.Status,
            CreditCardNumber = translator.CreditCardNumber
        };
    }
}
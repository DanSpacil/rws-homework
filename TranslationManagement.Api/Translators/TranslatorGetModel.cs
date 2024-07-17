namespace TranslationManagement.Api.Translators;

public record TranslatorGetModel
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string HourlyRate { get; init; }
    public string Status { get; init; }
    public string CreditCardNumber { get; init; }
}
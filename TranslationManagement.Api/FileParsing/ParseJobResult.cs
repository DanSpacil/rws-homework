namespace TranslationManagement.Api.FileParsing;

public class ParseJobResult
{
    public string CustomerName { get; private set; }

    public string OriginalContent { get; private set; }

    public string FailReason { get; private set; }

    public bool IsSuccess { get; private set; }

    public static ParseJobResult Error(string reason)
    {
        return new ParseJobResult { IsSuccess = false, FailReason = reason ?? string.Empty };
    }

    public static ParseJobResult Success(string customerName, string originalContent)
    {
        return new ParseJobResult { CustomerName = customerName, OriginalContent = originalContent };
    }
}
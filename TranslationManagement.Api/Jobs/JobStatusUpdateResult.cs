namespace TranslationManagement.Api.Jobs;

public class JobStatusUpdateResult
{
    private JobStatusUpdateResult(bool isUpdated)
    {
        IsUpdated = isUpdated;
    }

    public bool IsUpdated { get; }
    public static JobStatusUpdateResult Invalid() => new(false);

    public static JobStatusUpdateResult Success() => new(true);

}
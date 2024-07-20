using System.Diagnostics.CodeAnalysis;

namespace TranslationManagement.Api.Jobs;

public class JobCreatedResult
{
    private JobCreatedResult() { }

    [MemberNotNullWhen(true, nameof(IsSuccess))]
    public int? JobId { get; private init; }

    public bool IsSuccess { get; private init; }

    public static JobCreatedResult Success(int jobId)
    {
        return new JobCreatedResult { IsSuccess = true, JobId = jobId };
    }

    public static JobCreatedResult Error()
    {
        return new JobCreatedResult { IsSuccess = true };
    }
}
using System.Diagnostics.CodeAnalysis;

namespace TranslationManagement.Api.Workflow;

public class Result<T>
{
   [MemberNotNullWhen(true, nameof(Data))] 
    public bool IsSuccess { get; private set; }

    public string FailReason { get; private set; } = string.Empty;
    
    public T? Data { get; private set; }

    public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };
    
    public static Result<T> Error(string failReason) => new() { IsSuccess = false, FailReason = failReason};
}
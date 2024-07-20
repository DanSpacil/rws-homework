namespace TranslationManagement.Api.Workflow;

public class Result
{
    public bool IsSuccess { get; private set; }    
    
    public string FailReason { get; private set; }
}
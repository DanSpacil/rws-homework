namespace TranslationManagement.Api.Notifications;

public record JobCreatedNotification(int JobId) : INotification
{
    public string BuildMessage()
    {
        return "Job created: " + JobId;
    }
}
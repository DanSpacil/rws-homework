using System.Threading.Tasks;

namespace TranslationManagement.Api.Notifications;

public interface INotifier
{
    Task Notify(INotification notification);
}
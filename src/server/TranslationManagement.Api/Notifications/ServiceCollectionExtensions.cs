using External.ThirdParty.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TranslationManagement.Api.Notifications;

public static class ServiceCollectionExtensions
{
    public static void AddNotifications(this IServiceCollection services)
    {
        services.AddTransient<INotificationService, UnreliableNotificationService>();
        services.AddTransient<INotifier, ReliableNotifier>();
    }
}
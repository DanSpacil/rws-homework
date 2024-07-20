using System;
using System.Threading.Tasks;
using External.ThirdParty.Services;
using Microsoft.Extensions.Logging;

namespace TranslationManagement.Api.Notifications;

public class ReliableNotifier : INotifier
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<ReliableNotifier> _logger;

    public ReliableNotifier(INotificationService notificationService, ILogger<ReliableNotifier> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Notify(INotification notification)
    {
        var message = notification.BuildMessage();
        var shouldRetry = true;
        while (shouldRetry)
        {
            try
            {
                var wasSendSuccessful = await _notificationService.SendNotification(message);
                if (wasSendSuccessful)
                {
                    shouldRetry = false;
                    _logger.LogInformation("New job notification sent");
                }
            }
            catch (ApplicationException e)
            {
                _logger.LogTrace("Unreliable service failed with exception {Exception}", e);
            }
        }
    }
}
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class TariffNotificationService : BackgroundService
{
    private readonly SmsService _smsService;
    private readonly ILogger<TariffNotificationService> _logger;

    public TariffNotificationService(SmsService smsService, ILogger<TariffNotificationService> logger)
    {
        _smsService = smsService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var currentTime = DateTime.Now.TimeOfDay;

            // Check for tariff change times and send SMS
            var notificationMessage = GetTariffChangeNotification(currentTime);
            if (notificationMessage != null)
            {
                _smsService.SendSms("+353871146998", notificationMessage);
            }

            // Wait for a minute before checking again
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private string GetTariffChangeNotification(TimeSpan currentTime)
    {
        // Define your tariff change times and rates
        var peakStart = new TimeSpan(17, 0, 0); // 5 PM
        var peakEnd = new TimeSpan(19, 0, 0); // 7 PM
        var nightStart = new TimeSpan(23, 0, 0); // 11 PM
        var dayStart = new TimeSpan(8, 0, 0); // 8 AM

        var fifteenMinutes = TimeSpan.FromMinutes(15);

        // Check if current time is near any of the tariff change times
        if (currentTime.Add(fifteenMinutes) >= peakStart && currentTime.Add(fifteenMinutes) < peakEnd)
        {
            return "Alert: Peak electricity tariff period starting soon at 5 PM. Rate: 30p/kWh.";
        }
        else if (currentTime.Add(fifteenMinutes) >= nightStart || currentTime.Add(fifteenMinutes) < dayStart)
        {
            return "Alert: Night electricity tariff period starting soon at 11 PM. Rate: 20p/kWh.";
        }
        else if (currentTime.Add(fifteenMinutes) >= dayStart && currentTime.Add(fifteenMinutes) < peakStart)
        {
            return "Alert: Day electricity tariff period starting soon at 8 AM. Rate: 25p/kWh.";
        }

        return null; // No notification needed at this time
    }
}

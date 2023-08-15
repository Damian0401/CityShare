namespace CityShare.Backend.Application.Core.Models.Triggers;

public class TimerTriggerModel
{
    public TimerScheduleStatus ScheduleStatus { get; set; } = default!;

    public bool IsPastDue { get; set; }
}

public class TimerScheduleStatus
{
    public DateTime Last { get; set; }

    public DateTime Next { get; set; }

    public DateTime LastUpdated { get; set; }
}
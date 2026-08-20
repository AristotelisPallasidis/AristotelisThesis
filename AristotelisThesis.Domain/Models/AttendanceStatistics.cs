namespace AristotelisThesis.Domain.Models
{
    public class AttendanceStatistics : DomainObject
    {
        public int StudentId { get; set; }
        public double MonthlyAttendancePercentage { get; set; }
        public TimeSpan TodayActiveTime { get; set; }
        public int DaysAttendedThisWeek { get; set; }
        public double MonthlyActiveHours { get; set; }
        public int WeekLoginStreak { get; set; }
        public TimeSpan WeeklyAverageActiveTime { get; set; }

        public DateTime? TodayCheckIn { get; set; }

        // One entry per day of the current Monday-based week; always exactly 7 points.
        public virtual ICollection<WeeklyAttendanceDataPoint> WeeklyAttendanceGraph { get; set; }
        public virtual Student Student { get; set; }
    }
}

using System;

namespace AristotelisThesis.Domain.Models
{
    public class WeeklyAttendanceDataPoint : DomainObject
    {
        public int AttendanceStatisticsId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan ActiveTime { get; set; }
        public virtual AttendanceStatistics AttendanceStatistics { get; set; }
    }
}

using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace AristotelisThesis.EntityFramework.Services
{
    /// <summary>
    /// Computes the <see cref="AttendanceStatistics"/> read-model from a student's
    /// persisted <see cref="SessionHistory"/> rows. Nothing here is stored; the result
    /// is a fresh projection each time the Statistics page is opened.
    /// </summary>
    public class StatisticsService : IStatisticsService
    {
        private readonly AristotelisThesisDbContextFactory _contextFactory;

        public StatisticsService(AristotelisThesisDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<AttendanceStatistics> GetForStudent(int studentId)
        {
            using AristotelisThesisDbContext context = _contextFactory.CreateDbContext();

            List<SessionHistory> sessions = await context.SessionHistories
                .Where(s => s.StudentId == studentId)
                .ToListAsync();

            DateTime now = DateTime.Now;
            DateTime today = now.Date;
            DateTime weekStart = StartOfWeek(today);
            DateTime weekEnd = weekStart.AddDays(7);
            DateTime monthStart = new DateTime(today.Year, today.Month, 1);

            // Active time for a day; an open session (checked in, not yet out) counts live.
            TimeSpan EffectiveActive(SessionHistory s) =>
                s.Date.Date == today && s.CheckOut == null && s.CheckIn != null
                    ? now - s.CheckIn.Value
                    : s.ActiveTime;

            HashSet<DateTime> attendedDates = sessions.Select(s => s.Date.Date).ToHashSet();

            // --- Today ---
            SessionHistory todaySession = sessions.FirstOrDefault(s => s.Date.Date == today);
            TimeSpan todayActive = todaySession != null ? EffectiveActive(todaySession) : TimeSpan.Zero;
            DateTime? todayCheckIn = todaySession?.CheckIn;

            // --- This week ---
            List<SessionHistory> weekSessions = sessions
                .Where(s => s.Date.Date >= weekStart && s.Date.Date < weekEnd)
                .ToList();
            int daysAttendedThisWeek = weekSessions.Select(s => s.Date.Date).Distinct().Count();

            TimeSpan weeklyAverage = TimeSpan.Zero;
            if (daysAttendedThisWeek > 0)
            {
                long avgTicks = (long)weekSessions.Average(s => EffectiveActive(s).Ticks);
                weeklyAverage = TimeSpan.FromTicks(avgTicks);
            }

            // --- Streak: consecutive attended days ending today (or yesterday if not yet in today) ---
            int streak = 0;
            DateTime cursor = attendedDates.Contains(today) ? today : today.AddDays(-1);
            while (attendedDates.Contains(cursor))
            {
                streak++;
                cursor = cursor.AddDays(-1);
            }

            // --- This month ---
            List<SessionHistory> monthSessions = sessions
                .Where(s => s.Date.Date >= monthStart && s.Date.Date <= today)
                .ToList();

            double monthlyActiveHours = monthSessions.Sum(s => EffectiveActive(s).TotalHours);

            int elapsedWeekdays = 0;
            for (DateTime d = monthStart; d <= today; d = d.AddDays(1))
            {
                if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                {
                    elapsedWeekdays++;
                }
            }

            int attendedWeekdays = monthSessions
                .Select(s => s.Date.Date)
                .Distinct()
                .Count(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday);

            double monthlyPercentage = elapsedWeekdays > 0
                ? attendedWeekdays / (double)elapsedWeekdays * 100.0
                : 0.0;

            // --- Weekly graph: one point per weekday Mon..Sun of the current week ---
            List<WeeklyAttendanceDataPoint> graph = new List<WeeklyAttendanceDataPoint>();
            for (int i = 0; i < 7; i++)
            {
                DateTime dayDate = weekStart.AddDays(i);
                SessionHistory daySession = weekSessions.FirstOrDefault(s => s.Date.Date == dayDate);
                graph.Add(new WeeklyAttendanceDataPoint
                {
                    Date = dayDate,
                    ActiveTime = daySession != null ? EffectiveActive(daySession) : TimeSpan.Zero
                });
            }

            return new AttendanceStatistics
            {
                StudentId = studentId,
                TodayActiveTime = todayActive,
                TodayCheckIn = todayCheckIn,
                DaysAttendedThisWeek = daysAttendedThisWeek,
                WeeklyAverageActiveTime = weeklyAverage,
                WeekLoginStreak = streak,
                MonthlyActiveHours = monthlyActiveHours,
                MonthlyAttendancePercentage = monthlyPercentage,
                WeeklyAttendanceGraph = graph
            };
        }

        private static DateTime StartOfWeek(DateTime date)
        {
            // Monday-based week.
            int diff = ((int)date.DayOfWeek + 6) % 7;
            return date.Date.AddDays(-diff);
        }
    }
}

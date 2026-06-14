using System;
using System.Linq;
using System.Threading.Tasks;
using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace AristotelisThesis.EntityFramework.Services
{
    public class SessionTrackingService : ISessionTrackingService
    {
        private readonly AristotelisThesisDbContextFactory _contextFactory;

        public SessionTrackingService(AristotelisThesisDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task RecordCheckIn(int studentId)
        {
            using AristotelisThesisDbContext context = _contextFactory.CreateDbContext();

            DateTime now = DateTime.Now;
            DateTime today = now.Date;

            SessionHistory session = await context.SessionHistories
                .FirstOrDefaultAsync(s => s.StudentId == studentId && s.Date == today);

            if (session == null)
            {
                // First login of the day: open a new session.
                session = new SessionHistory
                {
                    StudentId = studentId,
                    Date = today,
                    CheckIn = now,
                    ActiveTime = TimeSpan.Zero
                };
                await context.SessionHistories.AddAsync(session);
            }
            else
            {
                // Already attended today: keep the earliest check-in. The next check-out
                // measures its interval from the last check-out (see RecordCheckOut), so a
                // re-login does not retroactively recount time already accumulated.
                session.CheckIn ??= now;
                context.SessionHistories.Update(session);
            }

            await context.SaveChangesAsync();
        }

        public async Task RecordCheckOut(int studentId)
        {
            using AristotelisThesisDbContext context = _contextFactory.CreateDbContext();

            DateTime now = DateTime.Now;
            DateTime today = now.Date;

            SessionHistory session = await context.SessionHistories
                .FirstOrDefaultAsync(s => s.StudentId == studentId && s.Date == today);

            // Nothing to close if there is no open session for today.
            if (session == null || session.CheckIn == null)
            {
                return;
            }

            // Accumulate the elapsed interval since the last check-in, then close it.
            DateTime intervalStart = session.CheckOut ?? session.CheckIn.Value;
            TimeSpan elapsed = now - intervalStart;
            if (elapsed > TimeSpan.Zero)
            {
                session.ActiveTime += elapsed;
            }

            session.CheckOut = now;
            context.SessionHistories.Update(session);

            await context.SaveChangesAsync();
        }
    }
}

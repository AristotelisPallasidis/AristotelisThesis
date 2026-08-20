namespace AristotelisThesis.Domain.Services
{
    /// <summary>
    /// Records a student's daily attendance session (check-in / check-out).
    /// One <see cref="Models.SessionHistory"/> row is kept per student per day.
    /// </summary>
    public interface ISessionTrackingService
    {
        /// <summary>
        /// Called on a successful login. Creates today's session for the student
        /// (recording the check-in time) if it does not already exist.
        /// </summary>
        Task RecordCheckIn(int studentId);

        /// <summary>
        /// Called on logout. Stamps the check-out time and accumulates the elapsed
        /// time since the last check-in into the day's active time.
        /// </summary>
        Task RecordCheckOut(int studentId);
    }
}

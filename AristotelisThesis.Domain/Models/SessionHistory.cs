namespace AristotelisThesis.Domain.Models
{
    public class SessionHistory : DomainObject
    {
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan ActiveTime { get; set; } // Total time spent for that day

        // First check-in of the day (set on login) and latest check-out (set on logout).
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }

        public virtual Student Student { get; set; }
    }
}

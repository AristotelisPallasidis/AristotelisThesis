using System;
using System.Collections.Generic;

namespace AristotelisThesis.Domain.Models
{
    public class SessionHistory : DomainObject
    {
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan ActiveTime { get; set; } // Total time spent for that day
        public virtual Student Student { get; set; }
    }
}

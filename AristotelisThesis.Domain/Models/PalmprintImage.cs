using System;

namespace AristotelisThesis.Domain.Models
{
    public class PalmprintImage : DomainObject
    {
        public int StudentId { get; set; }
        public byte[] ImageData { get; set; }
        public DateTime DateCaptured { get; set; }
        public virtual Student Student { get; set; }
    }
}

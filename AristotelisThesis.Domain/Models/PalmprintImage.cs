using System;

namespace AristotelisThesis.Domain.Models
{
    public class PalmprintImage : DomainObject
    {
        public int StudentId { get; set; }
        public byte[] ImageData { get; set; }

        // The palmprint feature vector (Gabor texture features packed as bytes) used for
        // matching at login. Nullable so legacy rows survive until backfilled.
        public byte[]? Embedding { get; set; }

        public DateTime DateCaptured { get; set; }
        public virtual Student Student { get; set; }
    }
}

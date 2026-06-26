using System;

namespace AristotelisThesis.Domain.Models
{
    public class FaceImage : DomainObject
    {
        public int StudentId { get; set; }
        public byte[] ImageData { get; set; }

        // The 128-d ResNet-34 face embedding (128 floats packed as bytes) used for
        // matching at login. Nullable so legacy rows survive until backfilled.
        public byte[]? Embedding { get; set; }

        public DateTime DateCaptured { get; set; }
        public virtual Student Student { get; set; }
    }
}

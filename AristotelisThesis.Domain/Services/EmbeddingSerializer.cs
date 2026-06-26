using System;

namespace AristotelisThesis.Domain.Services
{
    /// <summary>
    /// Packs/unpacks a 128-d face embedding between <see cref="float"/>[] (used for matching)
    /// and <see cref="byte"/>[] (stored in the database). 128 floats => 512 bytes.
    /// </summary>
    public static class EmbeddingSerializer
    {
        public static byte[] ToBytes(float[] embedding)
        {
            byte[] bytes = new byte[embedding.Length * sizeof(float)];
            Buffer.BlockCopy(embedding, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static float[] ToFloats(byte[] bytes)
        {
            float[] embedding = new float[bytes.Length / sizeof(float)];
            Buffer.BlockCopy(bytes, 0, embedding, 0, bytes.Length);
            return embedding;
        }

        /// <summary>
        /// Euclidean (L2) distance between two embeddings. Lower means more similar;
        /// dlib's recommended match threshold is ~0.6.
        /// </summary>
        public static double Distance(float[] a, float[] b)
        {
            double sum = 0;
            int n = Math.Min(a.Length, b.Length);
            for (int i = 0; i < n; i++)
            {
                double d = a[i] - b[i];
                sum += d * d;
            }
            return Math.Sqrt(sum);
        }
    }
}

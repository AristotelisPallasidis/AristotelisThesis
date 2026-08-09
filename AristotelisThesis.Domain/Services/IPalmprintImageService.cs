using System.Collections.Generic;
using System.Threading.Tasks;

namespace AristotelisThesis.Domain.Services
{
    /// <summary>
    /// Provides access to stored palmprint images: the gallery plus the enrolled palm feature
    /// vectors used for palmprint-recognition login. Mirror of <see cref="IFaceImageService"/>.
    /// </summary>
    public interface IPalmprintImageService
    {
        /// <summary>
        /// Returns the raw bytes of the first stored palm image for the student, or null if none.
        /// </summary>
        Task<byte[]> GetFirstImageData(int studentId);

        /// <summary>
        /// Returns the raw bytes of every stored palm image for the student (oldest first).
        /// </summary>
        Task<IReadOnlyList<byte[]>> GetAllImageData(int studentId);

        /// <summary>
        /// Persists a captured palm: the JPEG image plus its feature vector.
        /// </summary>
        Task SavePalmprintImage(int studentId, byte[] imageData, float[] embedding);

        /// <summary>
        /// Returns every enrolled (StudentId, embedding) pair that has a stored embedding.
        /// </summary>
        Task<IReadOnlyList<(int StudentId, float[] Embedding)>> GetAllEmbeddings();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace AristotelisThesis.Domain.Services
{
    /// <summary>
    /// Provides access to stored face images: the student's profile photo plus the
    /// enrolled face embeddings used for face-recognition login.
    /// </summary>
    public interface IFaceImageService
    {
        /// <summary>
        /// Returns the raw bytes of the first stored face image for the student,
        /// or null if the student has no enrolled face image.
        /// </summary>
        Task<byte[]> GetFirstImageData(int studentId);

        /// <summary>
        /// Returns the raw bytes of every stored face image for the student (oldest first).
        /// </summary>
        Task<IReadOnlyList<byte[]>> GetAllImageData(int studentId);

        /// <summary>
        /// Persists a captured face: the JPEG image plus its 128-d embedding.
        /// </summary>
        Task SaveFaceImage(int studentId, byte[] imageData, float[] embedding);

        /// <summary>
        /// Returns every enrolled (StudentId, embedding) pair that has a stored embedding.
        /// Used to match a probe face at login time.
        /// </summary>
        Task<IReadOnlyList<(int StudentId, float[] Embedding)>> GetAllEmbeddings();
    }
}

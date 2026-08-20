namespace AristotelisThesis.Domain.Services
{
    /// <summary>
    /// Provides access to stored palmprint images: the gallery plus the enrolled palm feature
    /// vectors used for palmprint-recognition login. Mirror of <see cref="IFaceImageService"/>.
    /// </summary>
    public interface IPalmprintImageService
    {
        /// <summary>
        /// Returns the raw bytes of every stored palm image for the student (oldest first).
        /// </summary>
        Task<IReadOnlyList<byte[]>> GetAllImageData(int studentId);

        /// <summary>
        /// Persists a set of captured palms - each JPEG plus its feature vector - in a single
        /// save, so an enrolment is never left half-written.
        /// </summary>
        Task SavePalmprintImages(int studentId, IReadOnlyList<(byte[] ImageData, float[] Embedding)> images);

        /// <summary>
        /// Returns every enrolled (StudentId, embedding) pair that has a stored embedding.
        /// </summary>
        Task<IReadOnlyList<(int StudentId, float[] Embedding)>> GetAllEmbeddings();
    }
}

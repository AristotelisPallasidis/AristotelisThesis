using System.Threading.Tasks;

namespace AristotelisThesis.Domain.Services
{
    /// <summary>
    /// Bridge to the Python face-embedding service. Turns a face image into a 128-d
    /// ResNet-34 embedding. Matching/storage is done elsewhere (this only encodes).
    /// </summary>
    public interface IFaceRecognitionService
    {
        /// <summary>
        /// Encodes the largest face in the given image (JPEG/PNG bytes) into a 128-d embedding,
        /// or returns null when no face is detected or the service is unreachable.
        /// </summary>
        Task<float[]?> EncodeAsync(byte[] imageBytes);

        /// <summary>
        /// True when the Python service is up and answering /health.
        /// </summary>
        Task<bool> IsHealthyAsync();
    }
}

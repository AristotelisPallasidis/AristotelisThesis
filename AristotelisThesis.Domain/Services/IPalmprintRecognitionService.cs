namespace AristotelisThesis.Domain.Services
{
    /// <summary>
    /// Bridge to the Python palmprint-encoding service. Turns a palm image into a fixed-length
    /// texture feature vector. Matching/storage is done elsewhere (this only encodes).
    /// </summary>
    public interface IPalmprintRecognitionService
    {
        /// <summary>
        /// Encodes the palm in the given image (JPEG/PNG bytes) into a feature vector, or
        /// returns null when no hand is detected or the service is unreachable.
        /// </summary>
        Task<float[]?> EncodeAsync(byte[] imageBytes);

        /// <summary>
        /// True when the Python palmprint service is up and answering /health.
        /// </summary>
        Task<bool> IsHealthyAsync();
    }
}

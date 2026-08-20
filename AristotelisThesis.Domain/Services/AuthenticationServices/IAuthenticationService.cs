using AristotelisThesis.Domain.Models;

namespace AristotelisThesis.Domain.Services.AuthenticationServices
{
    /// <summary>
    /// Authentication is biometric-only: accounts are created by the registration wizard
    /// (see Register06WithFaceViewModel) and signed in by matching a face or palm embedding.
    /// There is no username/password path.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Matches a probe face embedding against the enrolled embeddings and returns the
        /// owning <see cref="Account"/> when the best match is within the recognition threshold,
        /// or null when no enrolled face is close enough.
        /// </summary>
        Task<Account?> LoginWithFace(float[] probeEmbedding);

        /// <summary>
        /// Matches a probe palmprint embedding against the enrolled embeddings and returns the
        /// owning <see cref="Account"/> when the best match is within the recognition threshold,
        /// or null when no enrolled palm is close enough.
        /// </summary>
        Task<Account?> LoginWithPalmprint(float[] probeEmbedding);


    }
}

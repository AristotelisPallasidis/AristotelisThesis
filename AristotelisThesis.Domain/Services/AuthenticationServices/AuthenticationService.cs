using AristotelisThesis.Domain.Models;

namespace AristotelisThesis.Domain.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        // Max average L2 distance for a face match. dlib's 0.6 separates "same person vs.
        // random stranger" but is too loose for lookalikes/siblings, so we use a stricter 0.45.
        // Lower = stricter (fewer false accepts, more false rejects).
        private const double FaceMatchThreshold = 0.45;

        // Max average L2 distance for a palmprint match. The palm feature vectors are
        // L2-normalized, so distances run ~0..1.4; 0.6 is a starting point - tune with real
        // captures. Lower = stricter (fewer false accepts, more false rejects).
        private const double PalmprintMatchThreshold = 0.6;

        private readonly IAccountService _accountService;
        private readonly IFaceImageService _faceImageService;
        private readonly IPalmprintImageService _palmImageService;

        public AuthenticationService(IAccountService accountService, IFaceImageService faceImageService, IPalmprintImageService palmImageService)
        {
            _accountService = accountService;
            _faceImageService = faceImageService;
            _palmImageService = palmImageService;
        }


        /// <summary>
        /// Matches a probe face embedding against all enrolled embeddings using Euclidean
        /// distance and returns the owning account when the closest match is within the
        /// recognition threshold; otherwise returns null.
        /// </summary>
        public async Task<Account?> LoginWithFace(float[] probeEmbedding)
        {
            if (probeEmbedding == null || probeEmbedding.Length == 0)
            {
                return null;
            }

            IReadOnlyList<(int StudentId, float[] Embedding)> enrolled = await _faceImageService.GetAllEmbeddings();

            // Score each enrolled student by the AVERAGE distance to their photos (more robust
            // than the single closest photo, which lets a lookalike match on one lucky frame),
            // then take the best-scoring student.
            var ranked = enrolled
                .GroupBy(e => e.StudentId)
                .Select(g => new
                {
                    StudentId = g.Key,
                    Score = g.Average(e => EmbeddingSerializer.Distance(probeEmbedding, e.Embedding))
                })
                .OrderBy(x => x.Score)
                .ToList();

            if (ranked.Count == 0 || ranked[0].Score > FaceMatchThreshold)
            {
                return null;
            }

            return await _accountService.GetByStudentId(ranked[0].StudentId);
        }

        /// <summary>
        /// Matches a probe palmprint embedding against all enrolled palm embeddings using the
        /// average L2 distance per student and returns the owning account when the closest match
        /// is within the recognition threshold; otherwise returns null.
        /// </summary>
        public async Task<Account?> LoginWithPalmprint(float[] probeEmbedding)
        {
            if (probeEmbedding == null || probeEmbedding.Length == 0)
            {
                return null;
            }

            IReadOnlyList<(int StudentId, float[] Embedding)> enrolled = await _palmImageService.GetAllEmbeddings();

            var ranked = enrolled
                .GroupBy(e => e.StudentId)
                .Select(g => new
                {
                    StudentId = g.Key,
                    Score = g.Average(e => EmbeddingSerializer.Distance(probeEmbedding, e.Embedding))
                })
                .OrderBy(x => x.Score)
                .ToList();

            if (ranked.Count == 0 || ranked[0].Score > PalmprintMatchThreshold)
            {
                return null;
            }

            return await _accountService.GetByStudentId(ranked[0].StudentId);
        }

    }
}

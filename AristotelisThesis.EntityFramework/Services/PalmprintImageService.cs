using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace AristotelisThesis.EntityFramework.Services
{
    public class PalmprintImageService : IPalmprintImageService
    {
        private readonly AristotelisThesisDbContextFactory _contextFactory;

        public PalmprintImageService(AristotelisThesisDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IReadOnlyList<byte[]>> GetAllImageData(int studentId)
        {
            using AristotelisThesisDbContext context = _contextFactory.CreateDbContext();

            return await context.PalmprintImages
                .Where(p => p.StudentId == studentId)
                .OrderBy(p => p.Id)
                .Select(p => p.ImageData)
                .ToListAsync();
        }

        public async Task SavePalmprintImages(int studentId, IReadOnlyList<(byte[] ImageData, float[] Embedding)> images)
        {
            if (images == null || images.Count == 0)
            {
                return;
            }

            using AristotelisThesisDbContext context = _contextFactory.CreateDbContext();

            DateTime captured = DateTime.Now;
            foreach ((byte[] imageData, float[] embedding) in images)
            {
                context.PalmprintImages.Add(new PalmprintImage
                {
                    StudentId = studentId,
                    ImageData = imageData,
                    Embedding = EmbeddingSerializer.ToBytes(embedding),
                    DateCaptured = captured
                });
            }

            // One SaveChangesAsync, so an enrolment is stored whole or not at all.
            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<(int StudentId, float[] Embedding)>> GetAllEmbeddings()
        {
            using AristotelisThesisDbContext context = _contextFactory.CreateDbContext();

            List<(int StudentId, byte[] Embedding)> rows = await context.PalmprintImages
                .Where(p => p.Embedding != null)
                .Select(p => new ValueTuple<int, byte[]>(p.StudentId, p.Embedding))
                .ToListAsync();

            return rows
                .Select(r => (r.StudentId, EmbeddingSerializer.ToFloats(r.Embedding)))
                .ToList();
        }
    }
}

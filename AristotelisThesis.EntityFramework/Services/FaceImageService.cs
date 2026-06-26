using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace AristotelisThesis.EntityFramework.Services
{
    public class FaceImageService : IFaceImageService
    {
        private readonly AristotelisThesisDbContextFactory _contextFactory;

        public FaceImageService(AristotelisThesisDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<byte[]> GetFirstImageData(int studentId)
        {
            using AristotelisThesisDbContext context = _contextFactory.CreateDbContext();

            return await context.FaceImages
                .Where(f => f.StudentId == studentId)
                .OrderBy(f => f.Id)
                .Select(f => f.ImageData)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<byte[]>> GetAllImageData(int studentId)
        {
            using AristotelisThesisDbContext context = _contextFactory.CreateDbContext();

            return await context.FaceImages
                .Where(f => f.StudentId == studentId)
                .OrderBy(f => f.Id)
                .Select(f => f.ImageData)
                .ToListAsync();
        }

        public async Task SaveFaceImage(int studentId, byte[] imageData, float[] embedding)
        {
            using AristotelisThesisDbContext context = _contextFactory.CreateDbContext();

            context.FaceImages.Add(new FaceImage
            {
                StudentId = studentId,
                ImageData = imageData,
                Embedding = EmbeddingSerializer.ToBytes(embedding),
                DateCaptured = DateTime.Now
            });

            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<(int StudentId, float[] Embedding)>> GetAllEmbeddings()
        {
            using AristotelisThesisDbContext context = _contextFactory.CreateDbContext();

            List<(int StudentId, byte[] Embedding)> rows = await context.FaceImages
                .Where(f => f.Embedding != null)
                .Select(f => new ValueTuple<int, byte[]>(f.StudentId, f.Embedding))
                .ToListAsync();

            return rows
                .Select(r => (r.StudentId, EmbeddingSerializer.ToFloats(r.Embedding)))
                .ToList();
        }
    }
}

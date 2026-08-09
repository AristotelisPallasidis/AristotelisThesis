using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<byte[]> GetFirstImageData(int studentId)
        {
            using AristotelisThesisDbContext context = _contextFactory.CreateDbContext();

            return await context.PalmprintImages
                .Where(p => p.StudentId == studentId)
                .OrderBy(p => p.Id)
                .Select(p => p.ImageData)
                .FirstOrDefaultAsync();
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

        public async Task SavePalmprintImage(int studentId, byte[] imageData, float[] embedding)
        {
            using AristotelisThesisDbContext context = _contextFactory.CreateDbContext();

            context.PalmprintImages.Add(new PalmprintImage
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

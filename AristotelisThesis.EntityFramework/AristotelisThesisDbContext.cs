using AristotelisThesis.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AristotelisThesis.EntityFramework
{
    public class AristotelisThesisDbContext : DbContext
    {
        public AristotelisThesisDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public DbSet<FaceImage> FaceImages { get; set; }

        // Enrolled palmprint images + feature vectors (mirror of FaceImages).
        public DbSet<PalmprintImage> PalmprintImages { get; set; }

        // Daily attendance ledger; source of truth for the Statistics page.
        public DbSet<SessionHistory> SessionHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FaceImage>(b =>
            {
                b.Property(f => f.ImageData).IsRequired();
                // 128-d ResNet-34 embedding (512 bytes); nullable until a row is enrolled/backfilled.
                b.Property(f => f.Embedding).IsRequired(false);
                b.Property(f => f.DateCaptured).HasDefaultValueSql("CURRENT_TIMESTAMP");
                b.HasOne(f => f.Student)
                 .WithMany()
                 .HasForeignKey(f => f.StudentId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // PalmprintImage: same shape as FaceImage; cascades when the student is removed.
            modelBuilder.Entity<PalmprintImage>(b =>
            {
                b.Property(p => p.ImageData).IsRequired();
                b.Property(p => p.Embedding).IsRequired(false);
                b.Property(p => p.DateCaptured).HasDefaultValueSql("CURRENT_TIMESTAMP");
                b.HasOne(p => p.Student)
                 .WithMany()
                 .HasForeignKey(p => p.StudentId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // SessionHistory: one row per student per day; cascades when the student is removed.
            modelBuilder.Entity<SessionHistory>(b =>
            {
                b.HasOne(s => s.Student)
                 .WithMany()
                 .HasForeignKey(s => s.StudentId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // AEM is the university's student number: unique by definition, so enforce it in
            // the database rather than relying on the model's documentation alone.
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.AEM)
                .IsUnique();
        }
    }
}

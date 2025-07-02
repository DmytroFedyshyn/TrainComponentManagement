using Microsoft.EntityFrameworkCore;
using TrainComponentManagement.DAL.Data.Seeding;
using TrainComponentManagement.DAL.Models;

namespace TrainComponentManagement.DAL.Data
{
    public class TrainComponentContext : DbContext
    {
        public TrainComponentContext(DbContextOptions<TrainComponentContext> options)
            : base(options) { }

        public DbSet<Component> Components { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Component>(entity =>
            {
                entity.ToTable(tb => tb
                    .HasCheckConstraint(
                        "CK_Component_Quantity_NonNegative",
                        "Quantity IS NULL OR Quantity >= 0"
                    )
                );

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.UniqueNumber)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.HasIndex(e => e.UniqueNumber)
                      .IsUnique();

                entity.HasIndex(e => e.Name);

                entity.HasIndex(e => new { e.CanAssignQuantity, e.Quantity });
            });

            ComponentDataSeeder.Seed(modelBuilder);
        }
    }
}

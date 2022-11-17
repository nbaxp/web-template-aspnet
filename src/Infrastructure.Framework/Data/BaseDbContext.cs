using Infrastructure.Framework.Extensions;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Framework;

public class BaseDbContext : DbContext
{
    public const string CreatedAt = nameof(CreatedAt);
    public const string Id = nameof(Id);
    public const string RowVersion = nameof(RowVersion);
    public const string UpdatedAt = nameof(UpdatedAt);

    static BaseDbContext()
    {
        LinqToDBForEFTools.Initialize();
    }

    public BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    public override int SaveChanges()
    {
        this.ChangeTracker.DetectChanges();
        var entries = this.ChangeTracker.Entries().Where(o => o.State == EntityState.Added || o.State == EntityState.Modified).ToList();
        foreach (var entry in entries)
        {
            if (entry.Properties.Any(o => o.Metadata.Name == RowVersion))
            {
                entry.Property(RowVersion).CurrentValue = Guid.NewGuid().ToString();
            }
            if (entry.State == EntityState.Added && entry.Properties.Any(o => o.Metadata.Name == "CreatedAt"))
            {
                entry.Property(CreatedAt).CurrentValue = DateTime.UtcNow;
            }
            if (entry.State == EntityState.Modified && entry.Properties.Any(o => o.Metadata.Name == "UpdatedAt"))
            {
                entry.Property(UpdatedAt).CurrentValue = DateTime.UtcNow;
            }
        }
        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigModel(modelBuilder);
    }

    private static void ConfigModel(ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes().ToList();
        foreach (var entity in entityTypes)
        {
            if (entity.GetProperties().Any(o => o.Name == Id))
            {
                modelBuilder.Entity(entity.Name).HasKey(Id);
                modelBuilder.Entity(entity.Name).Property(Id).ValueGeneratedNever();
            }
            if (entity.GetProperties().Any(o => o.Name == RowVersion))
            {
                modelBuilder.Entity(entity.Name).Property(RowVersion).IsConcurrencyToken().ValueGeneratedNever();
            }
            if (entity.GetProperties().Any(o => o.Name == CreatedAt))
            {
                modelBuilder.Entity(entity.Name).Property<DateTime>(CreatedAt).ValueGeneratedOnAdd();
            }
            if (entity.GetProperties().Any(o => o.Name == UpdatedAt))
            {
                modelBuilder.Entity(entity.Name).Property<DateTime?>(UpdatedAt).ValueGeneratedOnUpdate();
            }
            //if (entity.GetProperties().Any(o => o.Name == "Parent") && entity.GetProperties().Any(o => o.Name == "Children"))
            //{
            //    modelBuilder.Entity(entity.Name).HasOne("Parent").WithMany("Children").HasForeignKey(new string[] { "ParentId" }).OnDelete(DeleteBehavior.SetNull);
            //    if (entity.GetProperties().Any(o => o.Name == "Name"))
            //    {
            //        modelBuilder.Entity(entity.Name).Property("Name").IsRequired();
            //    }
            //    if (entity.GetProperties().Any(o => o.Name == "Number"))
            //    {
            //        modelBuilder.Entity(entity.Name).Property("Number").IsRequired();
            //    }
            //}
            modelBuilder.Entity(entity.Name).HasComment(entity.ClrType.GetDisplayName());
            foreach (var prop in entity.GetProperties())
            {
                if (prop.PropertyInfo != null)
                {
                    modelBuilder.Entity(entity.ClrType).Property(prop.Name).HasComment(prop.PropertyInfo.GetDisplayName());
                }
            }
        }
    }
}

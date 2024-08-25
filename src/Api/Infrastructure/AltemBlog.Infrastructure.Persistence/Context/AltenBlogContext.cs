using AtenBlog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AltemBlog.Infrastructure.Persistence.Context;


public class AltenBlogContext : DbContext
{
    public const string DefaultSchema = "dbo";
    public AltenBlogContext() { }
    public AltenBlogContext(DbContextOptions options) : base(options) { }

    public DbSet<EmailConfirmation> EmailConfirmation { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<Entry> Entries { get; set; }
    public DbSet<EntryVote> EtryVotes { get; set; }
    public DbSet<EntryFavorite> EntryFavorites { get; set; }

    public DbSet<EntryComment> EntryComments { get; set; }
    public DbSet<EntryCommentVote> EntryCommentsVotes { get; set; }
    public DbSet<EntryCommentFavorite> EntryCommentFavorites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configString = "Data Source=.\\SQLEXPRESS;Initial Catalog=altenblog;Integrated Security= True;TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(configString, cfg =>
            {
                cfg.EnableRetryOnFailure();
            });
        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        OnBeforeSave();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSave();
        return base.SaveChangesAsync(cancellationToken);
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnBeforeSave();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    private void OnBeforeSave()
    {
        var addedEntities = ChangeTracker.Entries()
                            .Where(i => i.State == EntityState.Added)
                            .Select(i => (BaseEntity)i.Entity);
        PrepareAddedEntities(addedEntities);

    }

    private void PrepareAddedEntities(IEnumerable<BaseEntity> addedEntities)
    {
        foreach (var entity in addedEntities)
        {
            if (entity.CreatedDate == DateTime.MinValue)
                entity.CreatedDate = DateTime.Now;
        }
    }
}

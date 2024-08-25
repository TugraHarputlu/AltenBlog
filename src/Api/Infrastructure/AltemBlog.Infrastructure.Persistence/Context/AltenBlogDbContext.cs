using AtenBlog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AltemBlog.Infrastructure.Persistence.Context;


public class AltenBlogDbContext : DbContext
{
    public const string DefaultSchema= "dbo";
    public AltenBlogDbContext()
    {

    }
    public AltenBlogDbContext(DbContextOptions options) : base(options) { }

    public DbSet<EmailConfirmation> EmailConfirmation { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<Entry> Entrys { get; set; }
    public DbSet<EntryVote> EtryVotes { get; set; }
    public DbSet<EntryFavorite> EntryFavorites { get; set; }

    public DbSet<EntryComment> EntryComments { get; set; }
    public DbSet<EntryCommentVote> EntryCommentsVotes { get; set; }
    public DbSet<EntryCommentFavorite> EntryCommentFavorites { get; set; }
}

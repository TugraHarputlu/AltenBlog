using AltemBlog.Infrastructure.Persistence.Context;
using AtenBlog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AltemBlog.Infrastructure.Persistence.EntityConfiguration.EntityComment
{
    public class EntryCommentFavoriteEntityConfiguration : BaseEntityConfiguration<EntryCommentFavorite>
    {
        public override void Configure(EntityTypeBuilder<EntryCommentFavorite> builder)
        {
            base.Configure(builder);// BaseEntityConfiguration cagiriyor
            builder.ToTable("entryCommentFavorite", AltenBlogContext.DefaultSchema);

            builder.HasOne(i => i.EntryComment)
                .WithMany(i => i.EntryCommentFavorites)
                .HasForeignKey(i => i.EntryCommentId);


            builder.HasOne(i => i.CreatedUser)
                    .WithMany(i => i.EntryCommentFavorites)
                    .HasForeignKey(i => i.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);// user oldugu yere ekledik, silme islemlerinin dogru calisabilmasi icin
        }
    }
}

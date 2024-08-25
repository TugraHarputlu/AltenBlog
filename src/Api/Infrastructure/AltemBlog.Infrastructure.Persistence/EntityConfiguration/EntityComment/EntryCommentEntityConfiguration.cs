using AltemBlog.Infrastructure.Persistence.Context;
using AtenBlog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AltemBlog.Infrastructure.Persistence.EntityConfiguration.EntityComment
{
    public class EntryCommentEntityConfiguration : BaseEntityConfiguration<EntryComment>
    {
        public override void Configure(EntityTypeBuilder<EntryComment> builder)
        {
            base.Configure(builder);// BaseEntityConfiguration cagiriyor
            builder.ToTable("entryComment", AltenBlogContext.DefaultSchema);

            builder.HasOne(i => i.CreatedUser)
                .WithMany(i => i.EntryComments)
                .HasForeignKey(i => i.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);// user oldugu yere ekledik, silme islemlerinin dogru calisabilmasi icin

            builder.HasOne(i => i.Entry)
                    .WithMany(i => i.EntryComments)
                    .HasForeignKey(i => i.EntryId);
        }
    }
}

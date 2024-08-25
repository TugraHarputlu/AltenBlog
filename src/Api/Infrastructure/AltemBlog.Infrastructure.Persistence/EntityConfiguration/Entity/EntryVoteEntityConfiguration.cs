using AltemBlog.Infrastructure.Persistence.Context;
using AtenBlog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AltemBlog.Infrastructure.Persistence.EntityConfiguration.Entity;

public class EntryVoteEntityConfiguration : BaseEntityConfiguration<EntryVote>
{

    public override void Configure(EntityTypeBuilder<EntryVote> builder)
    {
        base.Configure(builder);// BaseEntityConfiguration cagiriyor
        builder.ToTable("entryVote", AltenBlogContext.DefaultSchema);

        builder.HasOne(i => i.Entry)
            .WithMany(i => i.EntryVotes)
            .HasForeignKey(i => i.EntryId);


    }
}
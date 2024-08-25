using AltemBlog.Infrastructure.Persistence.Context;
using AtenBlog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltemBlog.Infrastructure.Persistence.EntityConfiguration
{
    public class EmailConfirmationEntityConfiguration:BaseEntityConfiguration<EmailConfirmation>
    {
        public override void Configure(EntityTypeBuilder<EmailConfirmation> builder)
        {
            base.Configure(builder);
            builder.ToTable("emailconfirmation", AltenBlogContext.DefaultSchema);
        }
    }
}

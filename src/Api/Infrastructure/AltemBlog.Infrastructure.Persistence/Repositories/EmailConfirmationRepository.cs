using AltemBlog.Infrastructure.Persistence.Context;
using AltenBlog.Api.Application.Interfaces.Repositories;
using AtenBlog.Api.Domain.Models;

namespace AltemBlog.Infrastructure.Persistence.Repositories
{
    public class EmailConfirmationRepository : GenericRepository<EmailConfirmation>, IEmailConfirmationRepository
    {
        public EmailConfirmationRepository(AltenBlogContext dbContext) : base(dbContext)
        {
        }
    }
}

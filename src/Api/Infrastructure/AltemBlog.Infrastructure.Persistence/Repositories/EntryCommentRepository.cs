using AltemBlog.Infrastructure.Persistence.Context;
using AltenBlog.Api.Application.Interfaces.Repositories;
using AtenBlog.Api.Domain.Models;

namespace AltemBlog.Infrastructure.Persistence.Repositories
{
    public class EntryCommentRepository : GenericRepository<EntryComment>, IEntryCommentRepository
    {
        public EntryCommentRepository(AltenBlogContext dbContext) : base(dbContext) { }

    }
}

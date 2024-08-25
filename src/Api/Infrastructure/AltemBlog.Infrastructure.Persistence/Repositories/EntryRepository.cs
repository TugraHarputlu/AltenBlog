using AltemBlog.Infrastructure.Persistence.Context;
using AltenBlog.Api.Application.Interfaces.Repositories;
using AtenBlog.Api.Domain.Models;

namespace AltemBlog.Infrastructure.Persistence.Repositories;

public class EntryRepository : GenericRepository<Entry>, IEntryRepository
{
    public EntryRepository(AltenBlogContext dbContext) : base(dbContext)
    {
    }
}

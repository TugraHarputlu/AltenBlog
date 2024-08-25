using AltemBlog.Infrastructure.Persistence.Context;
using AltenBlog.Api.Application.Interfaces.Repositories;
using AtenBlog.Api.Domain.Models;

namespace AltemBlog.Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    //GenericRepository dependency injektion ile olusturulmuyor,biz UserRepository dependency injektion ile olusturuyoruz
    // buna gelen contexi base iletmemit gerek
    public UserRepository(AltenBlogContext dbContext) : base(dbContext)
    {
    }
}

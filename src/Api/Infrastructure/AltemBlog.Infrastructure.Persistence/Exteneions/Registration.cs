using AltemBlog.Infrastructure.Persistence.Context;
using AltemBlog.Infrastructure.Persistence.Repositories;
using AltenBlog.Api.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltemBlog.Infrastructure.Persistence.Exteneions
{
    public static class Registration
    {
        public static IServiceCollection AddInfrasructureRegistrations(this IServiceCollection services,IConfiguration configuration)
        {
            var connStr = configuration["AltenBlogDbConnetionString"].ToString();
            services.AddDbContext<AltenBlogContext>(cfg =>
            {
                cfg.UseSqlServer(connStr, opt =>
                {
                    opt.EnableRetryOnFailure();
                });
            });
            //bir kere calistirdigimiz icin 
            var seeData = new SeedData();
            seeData.SeedAsync(configuration).GetAwaiter().GetResult();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEntryRepository, EntryRepository>();
            return services;
        }
    }
}

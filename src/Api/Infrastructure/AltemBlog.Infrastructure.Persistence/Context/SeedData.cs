//bogus ta sahte datalar olusturmak
using AltenBlog.Common.Infrastructore;
using AtenBlog.Api.Domain.Models;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AltemBlog.Infrastructure.Persistence.Context
{
    internal class SeedData
    {
        private static List<User> GetUsers()//ilk isimiz user oluturuyoruz,user id ihtiyacimiz oldugundan
        {
            var result = new Faker<User>("de")
                .RuleFor(i => i.Id, i => Guid.NewGuid())
                .RuleFor(i => i.CreatedDate, i => i.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
                .RuleFor(i => i.FirstName, i => i.Person.FirstName)
                .RuleFor(i => i.LastName, i => i.Person.LastName)
                .RuleFor(i => i.Email, i => i.Internet.Email())
                .RuleFor(i => i.UserName, i => i.Internet.UserName())
                .RuleFor(i => i.Password, i => PasswordEncryptor.Encrpt(i.Internet.Password()))//sifreleri encryptor yapmak icin
                                                                                               //MD5 Geri dönülmez bir sifre olusturuyor
                .RuleFor(i => i.EmailConfirmed, i => i.PickRandom(true, false))
                .Generate(500);


            return result;

        }

        //datanin eklenmesi islemleri 
        public static async Task SeedAsync(IConfiguration configuration)
        {
            // burada dbcontexi olusturuyoruz
            var dbContextBuilder = new DbContextOptionsBuilder();
            dbContextBuilder.UseSqlServer(configuration["AltenBlogDbConnetionString"]);
            var contex = new AltenBlogContext(dbContextBuilder.Options);

            if (contex.Users.Any())
            {
                await Task.CompletedTask;
                return;
            }


            var users = GetUsers();
            var usersIds = users.Select(i => i.Id);

            await contex.Users.AddRangeAsync(users);

            //Bu giudler bize lazim olacagi icin bunslari hafizada tutmamiz lazim.
            var guids = Enumerable.Range(0, 150).Select(i => Guid.NewGuid()).ToList();
            int counter = 0;

            var entries = new Faker<Entry>("de")
                .RuleFor(i => i.Id, i => guids[counter++])
                .RuleFor(i => i.CreatedDate, i => i.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
                .RuleFor(i => i.Subject, i => i.Lorem.Sentence(5, 5))
                .RuleFor(i => i.Content, i => i.Lorem.Paragraph(2))
                .RuleFor(i => i.CreatedById, i => i.PickRandom(usersIds))
                .Generate(150);
            await contex.Entries.AddRangeAsync(entries);

            var comments = new Faker<EntryComment>(locale: "de")
                .RuleFor(i => i.Id, i => Guid.NewGuid())
                .RuleFor(i => i.CreatedDate, i => i.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
                .RuleFor(i => i.Content, i => i.Lorem.Paragraph(2))
                .RuleFor(i => i.CreatedById, i => i.PickRandom(usersIds))
                .RuleFor(i => i.EntryId, i => i.PickRandom(guids))
                .Generate(1000);

            await contex.EntryComments.AddRangeAsync(comments);

            await contex.SaveChangesAsync();

        }
    }
}

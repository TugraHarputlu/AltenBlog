using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AltenBlog.Api.Application.Extensions;

public static class Registration
{
    public static IServiceCollection AddApplicationRegistration(this IServiceCollection services)
    {
        var assm = Assembly.GetExecutingAssembly(); //WebApi bunu cagiracak, kendisine depen edecek,
        //dolayisiyla WebApi altindaki butun kütüphanaleri de altenda olacak

        services.AddAutoMapper(assm);
        services.AddValidatorsFromAssembly(assm);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assm));
        return services;
    }
}
using AltemBlog.Infrastructure.Persistence.Exteneions;
using AltenBlog.Api.Application.Extensions;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy = null; //burada herhangi bir name property kullanmasin
    })
    .AddFluentValidation();// bunu controllere FluentValidation icin eklememiz gerek.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrasructureRegistrations(builder.Configuration);

builder.Services.AddApplicationRegistration();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

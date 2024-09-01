using AltenBlog.Api.Application.Interfaces.Repositories;
using AltenBlog.Common.Infrastructore;
using AltenBlog.Common.Infrastructore.Exceptions;
using AltenBlog.Common.Models.Queries;
using AltenBlog.Common.Models.RequestModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AltenBlog.Api.Application.Features.Commands.User.Login;

public class LoginUserCommnetHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel> //LoginUserCommand alacak ve rückgabe degeri LoginUserViewModel
{
    private readonly IUserRepository userRepositiry;
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;

    public LoginUserCommnetHandler(IUserRepository userRepositiry, IMapper mapper, IConfiguration configuration)
    {
        this.userRepositiry = userRepositiry;
        this.mapper = mapper;
        this.configuration = configuration;
    }

    public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var dbUser = await userRepositiry.GetSingleAsync(i => i.Email == request.EmailAddress);
        if (dbUser == null)
            throw new DatabaseValidationException("User not found!!");

        // Kullanici var passwordu kontrol etmemiz gerek, passwordu encrpt ederke karsilastiriyoruz
        // cünkü biz passwordlari encrpt ederek saklamistik.
        var pass = PasswordEncryptor.Encrpt(request.Password);

        if (pass == null)
            throw new DatabaseValidationException("Password is Null or Empty!");

        if (dbUser.Password != pass)
            throw new DatabaseValidationException("Password is wrong!");

        if (!dbUser.EmailConfirmed)
            throw new DatabaseValidationException("Email address is not confirmed yet!");

        // Mappingin calisabilmasi icin bizim MappingProfile tanimlamamiz lazim
        var result = mapper.Map<LoginUserViewModel>(dbUser);

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
            new Claim(ClaimTypes.Email, dbUser.Email),
            new Claim(ClaimTypes.Name, dbUser.UserName),
            new Claim(ClaimTypes.GivenName, dbUser.FirstName),
            new Claim(ClaimTypes.Surname, dbUser.LastName),
        };

        result.Token = GenerateToken(claims);

        return result;

    }

    private string GenerateToken(Claim[] claims)
    {
        // appsettings.json key olusturmamiz gerek ki jwt tokunu sifrelerken bu key'yi klullanacagit
        // ayni key webblazor tarfindada olmasi lazim
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthConfig:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddDays(10);

        var token = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: creds, notBefore: DateTime.Now);
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        return jwtToken;
    }
}

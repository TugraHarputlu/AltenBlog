using AltenBlog.Common.Models.Queries;
using MediatR;

namespace AltenBlog.Common.Models.RequestModels;
// Burada bir tana Comment olusturuyoruz. Apülication tarafinda bu commentti kullanan bir tane handler yazmamiz gerek
// Bu commonlar webblazor tarafindanda kullanilacagi icin bunlari common altinda acacagiz.
public class LoginUserCommand : IRequest<LoginUserViewModel> // Geriye LoginUserViewModel dönecegini beliritoruz.
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }

    public LoginUserCommand(string emailAddress, string password)
    {
        EmailAddress = emailAddress;
        Password = password;
    }

    public LoginUserCommand()
    {

    }
}

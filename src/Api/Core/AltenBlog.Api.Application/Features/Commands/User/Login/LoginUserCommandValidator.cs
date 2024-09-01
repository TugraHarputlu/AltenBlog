using AltenBlog.Common.Models.RequestModels;
using FluentValidation;

namespace AltenBlog.Api.Application.Features.Commands.User.Login
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        //Birisi Controllere public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        // bir istek gönderirse daha conrollere istek gelmeden bunun validasyonunu gerceklestir.
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotNull()
                //AspNetCoreCompatible kullanarak bu emil adresinin bir mail adresi olup olmadigini kontrol etsin
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("{PropertyName} not a valid email address");

            RuleFor(x => x.Password)
                .NotNull()                                                      // buradaki karakteri veriyor
                .MinimumLength(6).WithMessage("{PropertyName} should at least be {MinLengt} characters");
        }
    }
}

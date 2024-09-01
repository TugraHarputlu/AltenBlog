using MediatR;

namespace AltenBlog.Common.Models.RequestModels;

// Bu commonlar webblazor tarafindanda kullanilacagi icin bunlari common altinda acacagiz.
public class CreateUserComment : IRequest<Guid>// Geriye Kullanicinin Guidni dönecegini beliritoruz.
{
    public string FirstName { get; set; } //Disaridan alinan bilgiler
    public string LastName { get; set; }//Disaridan alinan bilgiler
    public string EmailAddress { get; set; }//Disaridan alinan bilgiler
    public string Password { get; set; }//Disaridan alinan bilgiler
    public string UserName { get; set; }//Disaridan alinan bilgiler
}
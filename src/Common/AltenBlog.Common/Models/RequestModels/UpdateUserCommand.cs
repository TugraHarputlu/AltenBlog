using MediatR;

namespace AltenBlog.Common.Models.RequestModels;

// Bu commonlar webblazor tarafindanda kullanilacagi icin bunlari common altinda acacagiz.
public class UpdateUserCommand : IRequest<Guid> // Burada kullnicinin Guidini geriye dönüyoroz.
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }

    public string UserName { get; set; }
}

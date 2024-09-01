namespace AltenBlog.Common.Events.User;

public class UserEmailChangedEvent
{
    // burada id yi kullanmadik, cünkü email adresi de unik olacak buradan kullanici bulunabilinir
    public string OldEmailAdress { get; set; }
    public string NewEmailAdress { get; set; }
}
namespace AtenBlog.Api.Domain.Models
{
    public class EmailConfirmation : BaseEntity
    {
        public string OldMailAdresse { get; set; }
        public string NewMailAdresse { get; private set; }
    }
}

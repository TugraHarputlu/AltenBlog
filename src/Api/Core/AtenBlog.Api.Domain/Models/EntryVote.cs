using AltenBlog.Common.Models;

namespace AtenBlog.Api.Domain.Models
{
    public class EntryVote
    {
        public Guid EntryId { get; set; }
        public VoteType VoteType { get; set; }
        public Guid CreatedById { get; set; }
        public virtual Entry EntryComment { get; set; }
    }
}
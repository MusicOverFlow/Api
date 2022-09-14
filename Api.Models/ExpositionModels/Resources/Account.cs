namespace Api.Models.ExpositionModels.Resources;

public class AccountResource
{
    public string MailAddress { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Pseudonym { get; set; }
    public string PicUrl { get; set; }

    public ICollection<PostResource> OwnedPosts { get; set; }
    public ICollection<PostResource> LikedPosts { get; set; }
    public ICollection<CommentaryResource> LikedCommentaries { get; set; }
    public ICollection<CommentaryResource> OwnedCommentaries { get; set; }
    public ICollection<GroupResource> Groups { get; set; }
    public ICollection<AccountResource> Follows { get; set; }

    public DateTime CreatedAt { get; set; }
}

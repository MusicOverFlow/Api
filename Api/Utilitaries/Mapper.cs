using Api.Models.Entities;

namespace Api.Utilitaries;

public class Mapper
{
    public AccountResource AccountToResource(Account account)
    {
        if (account == null)
        {
            return null;
        }

        return new AccountResource()
        {
            MailAddress = account.MailAddress,
            Firstname = account.Firstname,
            Lastname = account.Lastname,
            CreatedAt = account.CreatedAt,
        };
    }

    public AccountResource AccountToResourceWithPostsAndCommentaries(Account account)
    {
        if (account == null)
        {
            return null;
        }

        List<PostResource> posts = new List<PostResource>();

        account.Posts.ToList().ForEach(post =>
        {
            posts.Add(this.PostToResourceWithCommentaries(post));
        });
        
        List<PostResource> commentaries = new List<PostResource>();

        account.Commentaries.ToList().ForEach(commentary =>
        {
            PostResource post = this.PostToResourceWithCommentaries(commentary.Post);
            if (!posts.Contains(post))
            {
                commentaries.Add(post);
            }
        });

        return new AccountResource()
        {
            MailAddress = account.MailAddress,
            Firstname = account.Firstname,
            Lastname = account.Lastname,
            CreatedAt = account.CreatedAt,
            Posts = posts,
        };
    }




    

    public PostResource PostToResource(Post post)
    {
        if (post == null)
        {
            return null;
        }

        return new PostResource()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            Account = this.AccountToResource(post.Account),
        };
    }

    public PostResource PostToResourceWithCommentaries(Post post)
    {
        if (post == null)
        {
            return null;
        }

        List<CommentaryResource> commentaries = new List<CommentaryResource>();

        post.Commentaries.ToList().ForEach(commentary =>
        {
            commentaries.Add(this.CommentaryToResource(commentary));
        });

        return new PostResource()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            Account = this.AccountToResource(post.Account),
            Commentaries = commentaries,
        };
    }





    

    public CommentaryResource CommentaryToResource(Commentary commentary)
    {
        if (commentary == null)
        {
            return null;
        }

        return new CommentaryResource()
        {
            Id = commentary.Id,
            Content = commentary.Content,
            CreatedAt = commentary.CreatedAt,
            Account = this.AccountToResource(commentary.Account),
        };
    }

    public CommentaryResource CommentaryToResourceWithPost(Commentary commentary)
    {
        if (commentary == null)
        {
            return null;
        }

        return new CommentaryResource()
        {
            Id = commentary.Id,
            Content = commentary.Content,
            CreatedAt = commentary.CreatedAt,
            Account = this.AccountToResource(commentary.Account),
            Post = this.PostToResource(commentary.Post),
        };
    }
}

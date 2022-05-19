using Api.Models.Entities;

namespace Api.Utilitaries;

public static class Mapper
{
    public static AccountResource AccountToResource(Account account)
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

    public static AccountResource AccountToResourceWithPostsAndCommentaries(Account account)
    {
        if (account == null)
        {
            return null;
        }

        List<PostResource> posts = new List<PostResource>();

        account.Posts.ToList().ForEach(post =>
        {
            posts.Add(Mapper.PostToResourceWithCommentaries(post));
        });
        
        List<PostResource> commentaries = new List<PostResource>();

        account.Commentaries.ToList().ForEach(commentary =>
        {
            PostResource post = Mapper.PostToResourceWithCommentaries(commentary.Post);
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




    

    public static PostResource PostToResource(Post post)
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
            Account = Mapper.AccountToResource(post.Account),
        };
    }

    public static PostResource PostToResourceWithCommentaries(Post post)
    {
        if (post == null)
        {
            return null;
        }

        List<CommentaryResource> commentaries = new List<CommentaryResource>();

        post.Commentaries.ToList().ForEach(commentary =>
        {
            commentaries.Add(Mapper.CommentaryToResource(commentary));
        });

        return new PostResource()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            Account = Mapper.AccountToResource(post.Account),
            Commentaries = commentaries,
        };
    }





    

    public static CommentaryResource CommentaryToResource(Commentary commentary)
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
            Account = Mapper.AccountToResource(commentary.Account),
        };
    }

    public static CommentaryResource CommentaryToResourceWithPost(Commentary commentary)
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
            Account = Mapper.AccountToResource(commentary.Account),
            Post = Mapper.PostToResource(commentary.Post),
        };
    }
}

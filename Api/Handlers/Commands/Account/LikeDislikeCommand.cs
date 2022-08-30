using Api.Handlers.Kernel;
using Api.Models.ExpositionModels.Requests;

namespace Api.Handlers.Commands;

public class LikeDislikeCommand : HandlerBase, Command<Task, LikeDislikeRequest>
{
    public LikeDislikeCommand(ModelsContext context) : base(context)
    {
        
    }

    public async Task Handle(LikeDislikeRequest likeRequest)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(likeRequest.CallerMail));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        Post post = await this.context.Posts
            .FirstOrDefaultAsync(p => p.Id.Equals(likeRequest.PostId));

        if (post != null)
        {
            if (post.Likes.Contains(account))
            {
                post.Likes.Remove(account);
                account.LikedPosts.Remove(post);
            }
            else
            {
                post.Likes.Add(account);
                account.LikedPosts.Add(post);
            }
            post.LikesCount = post.Likes.Count;
        }
        else
        {
            Commentary commentary = await this.context.Commentaries
                .FirstOrDefaultAsync(c => c.Id.Equals(likeRequest.PostId));

            if (commentary == null)
            {
                throw new HandlerException(ErrorType.PostOrCommentaryNotFound);
            }

            if (commentary.Likes.Contains(account))
            {
                commentary.Likes.Remove(account);
                account.LikedCommentaries.Remove(commentary);
            }
            else
            {
                commentary.Likes.Add(account);
                account.LikedCommentaries.Add(commentary);
            }
            commentary.LikesCount = post.Likes.Count;
        }

        await this.context.SaveChangesAsync();
    }
}

﻿using System.Security.Claims;

namespace Api.Controllers.CommentaryControllers;

public partial class CommentaryController
{
    [HttpPost]
    public async Task<ActionResult<CommentaryResource>> Create(CreateCommentary request, Guid? postId)
    {
        string mailAddress = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));
        
        if (account == null)
        {
            return NotFound(new { errorMessage = "Account not found" });
        }

        Post post = await this.context.Posts
            .FirstOrDefaultAsync(p => p.Id.Equals(postId));

        if (post == null)
        {
            return NotFound(new { errorMessage = "Post not found" });
        }

        Commentary commentary = new Commentary
        {
            Content = request.Content,
            CreatedAt = DateTime.Now,

            Owner = account,
            Post = post,
        };

        this.context.Commentaries.Add(commentary);
        post.Commentaries.Add(commentary);

        await this.context.SaveChangesAsync();

        return Created(nameof(Create), this.mapper.Post_ToResource(post));
    }
}

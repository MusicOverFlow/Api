using Api.Models.ExpositionModels.Resources;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpPut("addMusic"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<PostResource>> AddMusic(Guid? id)
    {
        /*
        Post post = await this.context.Posts
            .FirstOrDefaultAsync(p => p.Id.Equals(id));

        if (post == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.PostNotFound));
        }

        if (!Request.Form.Files.Any() || Request.Form.Files[0] == null || Request.Form.Files[0].Length == 0)
        {
            return BadRequest();
        }

        IFormFile file = Request.Form.Files[0];
        if (!DataValidator.IsSoundFormatSupported(file.FileName))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.WrongFormatFile));
        }

        using (var ms = new MemoryStream())
        {
            file.CopyTo(ms);
            byte[] fileBytes = ms.ToArray();

            post.MusicUrl = this.blob.GetMusicUrl(fileBytes, post.Id, file.FileName).Result;
        }

        await this.context.SaveChangesAsync();

        return Ok(this.mapper.Post_ToResource(post));
        */
        return Ok();
    }
}

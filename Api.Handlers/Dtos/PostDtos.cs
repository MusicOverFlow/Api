using Microsoft.AspNetCore.Http;

namespace Api.Handlers.Dtos;

public class AddMusicDto
{
    public Guid PostId { get; set; }
    public IFormFile File { get; set; }
}

public class CreatePostDto
{
    public string CreatorMailAddress { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid? GroupId { get; set; }
    public string ScriptLanguage { get; set; }
    public string Script { get; set; }
}

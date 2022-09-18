namespace Api.Handlers.Dtos;

public class CreateCommentaryDto
{
    public string CreatorMailAddress { get; set; }
    public string Content { get; set; }
    public Guid PostId { get; set; }
    public string ScriptLanguage { get; set; }
    public string Script { get; set; }
}

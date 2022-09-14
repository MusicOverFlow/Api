namespace Api.Models.ExpositionModels.Requests;

public class CreateCommentary
{
    [Required] public string Content { get; set; }
    public string ScriptLanguage { get; set; }
    public string Script { get; set; }
}

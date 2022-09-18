namespace Api.Models.ExpositionModels.Requests;

public class CreatePostRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string ScriptLanguage { get; set; }
    public string Script { get; set; }
}

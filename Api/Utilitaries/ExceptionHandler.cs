using System.Text.Json;

namespace Api.Utilitaries;

public class ExceptionHandler
{
    private Dictionary<ErrorType, ErrorDto> errors;

    public ExceptionHandler(string errorsFilepath)
    {
        this.errors = this.GetErrorsFromFile(errorsFilepath);
    }
    
    private Dictionary<ErrorType, ErrorDto> GetErrorsFromFile(string errorsFilepath)
    {
        string fileContent = File.ReadAllText(errorsFilepath);
        
        return JsonSerializer.Deserialize<Dictionary<ErrorType, ErrorDto>>(fileContent, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        });
    }

    public ErrorDto GetError(ErrorType errorType)
    {
        return this.errors.TryGetValue(errorType, out ErrorDto errorDto) ? errorDto : new ErrorDto();
    }
}

public class ErrorDto
{
    public string Error { get; init; } = "Error";
    public string Message { get; init; } = "An error occured";
    public string Example { get; init; } = string.Empty;
}

public enum ErrorType
{
    InvalidMail,
    InvalidPassword,
    MailAlreadyInUse,
    AccountNotFound,
    SelfFollow,
    PostOrCommentaryNotFound,
    InvalidName,
    InvalidPseudonym,
    InvalidRole,
    WrongCredentials,
    GroupeMissingName,
    GroupNotFound,
    NotMemberOfGroup,
    AccountAlreadyInGroup,
    LeaveWhileOwner,
    NotOwnerOfGroup,
    AccountNotInGroup,
    PostTitleOrContentEmpty,
    PostNotFound,
}
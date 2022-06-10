using System.Text.Json;

namespace Api.Utilitaries;

public class ExceptionHandler
{
    private Dictionary<string, Exception> exceptions;

    public ExceptionHandler(string exceptionsFilepath)
    {
        this.exceptions = this.ExceptionsRuntimeInitialization(exceptionsFilepath);
    }
    
    private Dictionary<string, Exception> ExceptionsRuntimeInitialization(string exceptionsFilepath)
    {
        return JsonSerializer.Deserialize<Dictionary<string, Exception>>(File.ReadAllText(exceptionsFilepath), new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        });
    }

    public Exception GetException(BadRequestType badRequestType)
    {
        return this.exceptions.TryGetValue(badRequestType.ToString(), out Exception exception) ? exception : new Exception();
    }
}

public class Exception
{
    public string Error { get; init; } = "Execution error";
    public string Message { get; init; } = "An error occured";
    public string Example { get; init; } = string.Empty;
}

public enum BadRequestType
{
    InvalidMail,
    InvalidPassword,
    MailAlreadyInUse,
    AccountNotFound,
    SelfFollow,
    PostOrCommentaryNotFound,
    InvalidName,
    InvalidRole,
    WrongCredentials,
    GroupeMissingName,
    GroupNotFound,
    AccountAlreadyInGroup,
    LeaveWhileOwner,
    NotOwnerOfGroup,
    AccountNotInGroup,
    PostTitleOrContentEmpty,
    PostNotFound,
}
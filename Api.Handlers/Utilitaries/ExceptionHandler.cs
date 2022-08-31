using System.Text.Json;

namespace Api.Handlers.Utilitaries;

public abstract class ExceptionHandler
{
    private static readonly Dictionary<ErrorType, ExceptionDto> exceptions = GetErrorsFromFile(new DirectoryInfo(Directory.GetCurrentDirectory()) + "/exceptions.json");

    private static Dictionary<ErrorType, ExceptionDto> GetErrorsFromFile(string errorsFilepath)
    {
        string fileContent = File.ReadAllText(errorsFilepath);

        return JsonSerializer.Deserialize<Dictionary<ErrorType, ExceptionDto>>(fileContent, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        });
    }

    public static ExceptionDto Get(ErrorType errorType)
    {
        return exceptions.TryGetValue(errorType, out ExceptionDto errorDto) ? new ExceptionDto()
        {
            Error = errorDto.Error,
            Message = errorDto.Message,
            Example = errorDto.Example,
        } : new ExceptionDto();
    }

    public static int GetCode(ErrorType errorType)
    {
        return exceptions.TryGetValue(errorType, out ExceptionDto errorDto) ? errorDto.Code : 500;
    }
}

public class ExceptionDto
{
    public int Code { get; init; }
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
    PostTitleOrContentEmpty,
    PostNotFound,
    WrongFormatFile,
}
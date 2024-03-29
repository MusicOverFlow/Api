﻿using System.Text.Json;

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
        return exceptions.TryGetValue(errorType, out ExceptionDto exceptionDto) ? new ExceptionDto()
        {
            Error = exceptionDto.Error,
            Message = exceptionDto.Message,
            Example = exceptionDto.Example,
        } : new ExceptionDto();
    }

    public static int GetCode(ErrorType errorType)
    {
        return exceptions.TryGetValue(errorType, out ExceptionDto exceptionDto) ? exceptionDto.Code : 500;
    }
}

public class ExceptionDto
{
    public int Code { get; init; }
    public string Error { get; init; } = "Erreur";
    public string Message { get; init; } = "Une erreur est survenue";
    public string Example { get; init; } = string.Empty;
}

public enum ErrorType
{
    Undefined,

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
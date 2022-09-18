using EmailValidation;
using EzPasswordValidator.Checks;
using EzPasswordValidator.Validators;

namespace Api.Handlers.Utilitaries;

public abstract class DataValidator
{
    private static readonly PasswordValidator passwordValidator = new PasswordValidator(
        CheckTypes.CaseUpperLower |
        CheckTypes.Numbers |
        CheckTypes.Symbols |
        CheckTypes.Length);

    public static bool IsMailAddressValid(string mailAddress)
    {
        return !string.IsNullOrWhiteSpace(mailAddress) && EmailValidator.Validate(mailAddress);
    }

    public static bool IsPasswordValid(string password)
    {
        passwordValidator.SetLengthBounds(4, 20);

        return !string.IsNullOrWhiteSpace(password) && passwordValidator.Validate(password);
    }

    public static bool IsSoundFormatSupported(string filePathOrName)
    {
        filePathOrName = Path.GetExtension(filePathOrName);
        return filePathOrName == ".mp3" || filePathOrName == ".wav";
    }

    public static bool IsImageFormatSupported(string filePathOrName)
    {
        filePathOrName = Path.GetExtension(filePathOrName);
        return filePathOrName == ".jpg" || filePathOrName == ".jpeg"
            || filePathOrName == ".png" || filePathOrName == ".bmp";
    }
}

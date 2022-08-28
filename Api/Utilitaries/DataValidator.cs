using EmailValidation;
using EzPasswordValidator.Checks;
using EzPasswordValidator.Validators;

namespace Api.Utilitaries;

public class DataValidator
{
    private readonly PasswordValidator passwordValidator;

    public DataValidator()
    {
        this.passwordValidator = new PasswordValidator(
            CheckTypes.CaseUpperLower |
            CheckTypes.Numbers |
            CheckTypes.Symbols |
            CheckTypes.Length);
    }

    public bool IsMailAddressValid(string mailAddress)
    {
        return !string.IsNullOrWhiteSpace(mailAddress) && EmailValidator.Validate(mailAddress);
    }

    public bool IsPasswordValid(string password)
    {
        this.passwordValidator.SetLengthBounds(4, 20);

        return !string.IsNullOrWhiteSpace(password) && this.passwordValidator.Validate(password);
    }

    public bool IsSoundFormatSupported(string filePathOrName)
    {
        filePathOrName = Path.GetExtension(filePathOrName);
        return filePathOrName == ".mp3" || filePathOrName == ".wav";
    }

    public bool IsImageFormatSupported(string filePathOrName)
    {
        filePathOrName = Path.GetExtension(filePathOrName);
        return filePathOrName == ".jpg" || filePathOrName == ".jpeg"
            || filePathOrName == ".png" || filePathOrName == ".bmp";
    }
}

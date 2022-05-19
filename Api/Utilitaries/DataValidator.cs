using EmailValidation;
using EzPasswordValidator.Checks;
using EzPasswordValidator.Validators;

namespace Api.Utilitaries;

public static class DataValidator
{
    private static PasswordValidator passwordValidator = new PasswordValidator(
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
        passwordValidator.SetLengthBounds(8, 20);

        return !string.IsNullOrWhiteSpace(password) && passwordValidator.Validate(password);
    }
}

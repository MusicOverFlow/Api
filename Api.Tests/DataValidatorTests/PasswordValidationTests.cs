using Api.Utilitaries;

namespace Api.Tests.DataValidatorTests;

public class PasswordValidationTests
{
    [Fact(DisplayName =
        "Password '123MyPass?'\n" +
        "Should be valid")]
    public void PasswordValidation_1()
    {
        DataValidator.IsPasswordValid("123MyPass?").Should().BeTrue();
    }

    [Fact(DisplayName =
        "Password '123MYPASS?'\n" +
        "Should not be valid\n" +
        "Because it is missing the lower case letter")]
    public void PasswordValidation_2()
    {
        DataValidator.IsPasswordValid("123MYPASS?").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Password '123mypass?'\n" +
        "Should not be valid\n" +
        "Because it is missing the upper case letter")]
    public void PasswordValidation_3()
    {
        DataValidator.IsPasswordValid("123mypass?").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Password 'MyPass?'\n" +
        "Should not be valid\n" +
        "Because it is missing the digit")]
    public void PasswordValidation_4()
    {
        DataValidator.IsPasswordValid("MyPass?").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Password '123MyPass'\n" +
        "Should not be valid\n" +
        "Because it is missing the symbol")]
    public void PasswordValidation_5()
    {
        DataValidator.IsPasswordValid("123MyPass").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Password '1Mp?'\n" +
        "Should not be valid\n" +
        "Because it is too short")]
    public void PasswordValidation_6()
    {
        DataValidator.IsPasswordValid("1Mp?").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Password '123456MyLongPassword?;:!'\n" +
        "Should not be valid\n" +
        "Because it is too long")]
    public void PasswordValidation_7()
    {
        DataValidator.IsPasswordValid("123456MyLongPassword?;:!").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Empty password\n" +
        "Should not be valid")]
    public void PasswordValidation_8()
    {
        DataValidator.IsPasswordValid("").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Null password\n" +
        "Should not be valid")]
    public void PasswordValidation_9()
    {
        DataValidator.IsPasswordValid(null).Should().BeFalse();
    }
}

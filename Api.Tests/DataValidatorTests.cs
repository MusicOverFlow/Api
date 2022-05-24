using Api.Utilitaries;
using FluentAssertions;
using Xunit;

namespace Api.Tests;

public class DataValidatorTests
{
    private readonly DataValidator dataValidator = new DataValidator();

    /**
     * Mail validation tests
     */
    [Fact(DisplayName =
        "Mail 'gtouchet@myges.fr'\n" +
        "Should be valid")]
    public void MailValidation_1()
    {
        this.dataValidator.IsMailAddressValid("gtouchet@myges.fr").Should().BeTrue();
    }

    [Fact(DisplayName =
        "Mail 'gtouchetmyges.fr'\n" +
        "Should not be valid\n" +
        "Because it is missing the '@'")]
    public void MailValidation_2()
    {
        this.dataValidator.IsPasswordValid("gtouchesmyges.fr").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Mail 'gtouches@mygesfr'\n" +
        "Should not be valid\n" +
        "Because it is missing the '.'")]
    public void MailValidation_3()
    {
        this.dataValidator.IsPasswordValid("gtouches@mygesfr").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Mail 'gtouches@'\n" +
        "Should not be valid\n" +
        "Because it is missing the domain")]
    public void MailValidation_4()
    {
        this.dataValidator.IsPasswordValid("gtouches@").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Mail '@myges.fr'\n" +
        "Should not be valid\n" +
        "Because it is missing the address's name")]
    public void MailValidation_5()
    {
        this.dataValidator.IsPasswordValid("@myges.fr").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Empty mail\n" +
        "Should not be valid")]
    public void MailValidation_6()
    {
        this.dataValidator.IsPasswordValid("").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Null parameter\n" +
        "Should not be valid")]
    public void MailValidation_7()
    {
        this.dataValidator.IsPasswordValid(null).Should().BeFalse();
    }

    /**
     * Password validation tests
     */
    [Fact(DisplayName =
        "Password '123MyPass?'\n" +
        "Should be valid")]
    public void PasswordValidation_1()
    {
        this.dataValidator.IsPasswordValid("123MyPass?").Should().BeTrue();
    }

    [Fact(DisplayName =
        "Password '123MYPASS?'\n" +
        "Should not be valid\n" +
        "Because it is missing the lower case letter")]
    public void PasswordValidation_2()
    {
        this.dataValidator.IsPasswordValid("123MYPASS?").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Password '123mypass?'\n" +
        "Should not be valid\n" +
        "Because it is missing the upper case letter")]
    public void PasswordValidation_3()
    {
        this.dataValidator.IsPasswordValid("123mypass?").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Password 'MyPass?'\n" +
        "Should not be valid\n" +
        "Because it is missing the digit")]
    public void PasswordValidation_4()
    {
        this.dataValidator.IsPasswordValid("MyPass?").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Password '123MyPass'\n" +
        "Should not be valid\n" +
        "Because it is missing the symbol")]
    public void PasswordValidation_5()
    {
        this.dataValidator.IsPasswordValid("123MyPass").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Password '1Mp?'\n" +
        "Should not be valid\n" +
        "Because it is too short")]
    public void PasswordValidation_6()
    {
        this.dataValidator.IsPasswordValid("1Mp?").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Password '123456MyLongPassword?;:!'\n" +
        "Should not be valid\n" +
        "Because it is too long")]
    public void PasswordValidation_7()
    {
        this.dataValidator.IsPasswordValid("123456MyLongPassword?;:!").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Empty password\n" +
        "Should not be valid")]
    public void PasswordValidation_8()
    {
        this.dataValidator.IsPasswordValid("").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Null password\n" +
        "Should not be valid")]
    public void PasswordValidation_9()
    {
        this.dataValidator.IsPasswordValid(null).Should().BeFalse();
    }
}

using Api.Utilitaries;

namespace Api.Tests.DataValidatorTests;

public class MailValidationTests
{
    private readonly DataValidator dataValidator = new DataValidator();

    [Fact(DisplayName =
        "Mail 'gtouchet@myges.fr'\n" +
        "Should be valid")]
    public void MailValidation_1()
    {
        dataValidator.IsMailAddressValid("gtouchet@myges.fr").Should().BeTrue();
    }

    [Fact(DisplayName =
        "Mail 'gtouchetmyges.fr'\n" +
        "Should not be valid\n" +
        "Because it is missing the '@'")]
    public void MailValidation_2()
    {
        dataValidator.IsPasswordValid("gtouchesmyges.fr").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Mail 'gtouches@mygesfr'\n" +
        "Should not be valid\n" +
        "Because it is missing the '.'")]
    public void MailValidation_3()
    {
        dataValidator.IsPasswordValid("gtouches@mygesfr").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Mail 'gtouches@'\n" +
        "Should not be valid\n" +
        "Because it is missing the domain")]
    public void MailValidation_4()
    {
        dataValidator.IsPasswordValid("gtouches@").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Mail '@myges.fr'\n" +
        "Should not be valid\n" +
        "Because it is missing the address's name")]
    public void MailValidation_5()
    {
        dataValidator.IsPasswordValid("@myges.fr").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Empty mail\n" +
        "Should not be valid")]
    public void MailValidation_6()
    {
        dataValidator.IsPasswordValid("").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Null parameter\n" +
        "Should not be valid")]
    public void MailValidation_7()
    {
        dataValidator.IsPasswordValid(null).Should().BeFalse();
    }
}

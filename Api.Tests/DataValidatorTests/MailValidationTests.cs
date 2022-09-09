namespace Api.Tests.DataValidatorTests;

public class MailValidationTests
{
    [Fact(DisplayName =
        "Mail 'gtouchet@myges.fr'\n" +
        "Should be valid")]
    public void MailValidation_1()
    {
        DataValidator.IsMailAddressValid("gtouchet@myges.fr").Should().BeTrue();
    }

    [Fact(DisplayName =
        "Mail 'gtouchetmyges.fr'\n" +
        "Should not be valid\n" +
        "Because it is missing the '@'")]
    public void MailValidation_2()
    {
        DataValidator.IsPasswordValid("gtouchetmyges.fr").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Mail 'gtouchet@mygesfr'\n" +
        "Should not be valid\n" +
        "Because it is missing the '.'")]
    public void MailValidation_3()
    {
        DataValidator.IsPasswordValid("gtouchet@mygesfr").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Mail 'gtouchet@'\n" +
        "Should not be valid\n" +
        "Because it is missing the domain")]
    public void MailValidation_4()
    {
        DataValidator.IsPasswordValid("gtouchet@").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Mail '@myges.fr'\n" +
        "Should not be valid\n" +
        "Because it is missing the address's name")]
    public void MailValidation_5()
    {
        DataValidator.IsPasswordValid("@myges.fr").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Empty mail\n" +
        "Should not be valid")]
    public void MailValidation_6()
    {
        DataValidator.IsPasswordValid("").Should().BeFalse();
    }

    [Fact(DisplayName =
        "Null parameter\n" +
        "Should not be valid")]
    public void MailValidation_7()
    {
        DataValidator.IsPasswordValid(null).Should().BeFalse();
    }
}

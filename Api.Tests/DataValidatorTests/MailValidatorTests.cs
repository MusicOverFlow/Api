using Api.Utilitaries;
using FluentAssertions;
using Xunit;

namespace Api.Tests.DataValidatorTests;

public class MailValidatorTests
{
    [Fact]
    public void Mail_Valid_ShouldBeValid()
    {
        DataValidator.IsMailAddressValid("gtouchet@myges.fr").Should().BeTrue();
    }

    [Fact]
    public void Mail_WithoutAt_ShouldBeInvalid()
    {
        DataValidator.IsPasswordValid("gtouchesmyges.fr").Should().BeFalse();
    }

    [Fact]
    public void Mail_WithoutDot_ShouldBeInvalid()
    {
        DataValidator.IsPasswordValid("gtouches@mygesfr").Should().BeFalse();
    }

    [Fact]
    public void Mail_WithoutDomain_ShouldBeInvalid()
    {
        DataValidator.IsPasswordValid("gtouches@").Should().BeFalse();
    }

    [Fact]
    public void Mail_WithoutIdentifier_ShoudBeInvalid()
    {
        DataValidator.IsPasswordValid("@myges.fr").Should().BeFalse();
    }

    [Fact]
    public void Mail_Empty_ShoudBeInvalid()
    {
        DataValidator.IsPasswordValid("").Should().BeFalse();
    }

    [Fact]
    public void Mail_Null_ShoudBeInvalid()
    {
        DataValidator.IsPasswordValid(null).Should().BeFalse();
    }
}

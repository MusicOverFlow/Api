using Api.Utilitaries;
using FluentAssertions;
using Xunit;

namespace Api.Tests;

public class DataValidatorTests
{
    DataValidator dataValidator = new DataValidator();

    /**
     * Mail validator tests
     */
    [Fact]
    public void Mail_Valid_ShouldBeValid()
    {
        this.dataValidator.IsMailAddressValid("gtouchet@myges.fr").Should().BeTrue();
    }

    [Fact]
    public void Mail_WithoutAt_ShouldBeInvalid()
    {
        this.dataValidator.IsPasswordValid("gtouchesmyges.fr").Should().BeFalse();
    }

    [Fact]
    public void Mail_WithoutDot_ShouldBeInvalid()
    {
        this.dataValidator.IsPasswordValid("gtouches@mygesfr").Should().BeFalse();
    }

    [Fact]
    public void Mail_WithoutDomain_ShouldBeInvalid()
    {
        this.dataValidator.IsPasswordValid("gtouches@").Should().BeFalse();
    }

    [Fact]
    public void Mail_WithoutIdentifier_ShoudBeInvalid()
    {
        this.dataValidator.IsPasswordValid("@myges.fr").Should().BeFalse();
    }

    [Fact]
    public void Mail_Empty_ShoudBeInvalid()
    {
        this.dataValidator.IsPasswordValid("").Should().BeFalse();
    }

    [Fact]
    public void Mail_Null_ShoudBeInvalid()
    {
        this.dataValidator.IsPasswordValid(null).Should().BeFalse();
    }

    /**
     * Password validator tests
     */
    [Fact]
    public void Password_With_LowerCase_UpperCase_Digit_Symbol_ShouldBeValid()
    {
        this.dataValidator.IsPasswordValid("123MyPass?").Should().BeTrue();
    }

    [Fact]
    public void Password_Without_LowerCase_ShouldBeInvalid()
    {
        this.dataValidator.IsPasswordValid("123MYPASS?").Should().BeFalse();
    }

    [Fact]
    public void Password_Without_UpperCase_ShouldBeInvalid()
    {
        this.dataValidator.IsPasswordValid("123mypass?").Should().BeFalse();
    }

    [Fact]
    public void Password_Without_Digit_ShouldBeInvalid()
    {
        this.dataValidator.IsPasswordValid("MyPass?").Should().BeFalse();
    }

    [Fact]
    public void Password_Without_Symbol_ShouldBeInvalid()
    {
        this.dataValidator.IsPasswordValid("123MyPass").Should().BeFalse();
    }

    [Fact]
    public void Password_TooShort_ShoudBeInvalid()
    {
        this.dataValidator.IsPasswordValid("1Mp?").Should().BeFalse();
    }

    [Fact]
    public void Password_TooLong_ShoudBeInvalid()
    {
        this.dataValidator.IsPasswordValid("123456MyLongPassword?;:!").Should().BeFalse();
    }

    [Fact]
    public void Password_Empty_ShoudBeInvalid()
    {
        this.dataValidator.IsPasswordValid("").Should().BeFalse();
    }

    [Fact]
    public void Password_Null_ShoudBeInvalid()
    {
        this.dataValidator.IsPasswordValid(null).Should().BeFalse();
    }
}

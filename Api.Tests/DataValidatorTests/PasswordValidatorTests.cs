using Api.Utilitaries;
using FluentAssertions;
using Xunit;

namespace Api.Tests.DataValidatorTests;

public class PasswordValidatorTests
{
    [Fact]
    public void Password_With_LowerCase_UpperCase_Digit_Symbol_ShouldBeValid()
    {
        DataValidator.IsPasswordValid("123MyPass?").Should().BeTrue();
    }

    [Fact]
    public void Password_Without_LowerCase_ShouldBeInvalid()
    {
        DataValidator.IsPasswordValid("123MYPASS?").Should().BeFalse();
    }

    [Fact]
    public void Password_Without_UpperCase_ShouldBeInvalid()
    {
        DataValidator.IsPasswordValid("123mypass?").Should().BeFalse();
    }

    [Fact]
    public void Password_Without_Digit_ShouldBeInvalid()
    {
        DataValidator.IsPasswordValid("MyPass?").Should().BeFalse();
    }

    [Fact]
    public void Password_Without_Symbol_ShouldBeInvalid()
    {
        DataValidator.IsPasswordValid("123MyPass").Should().BeFalse();
    }

    [Fact]
    public void Password_TooShort_ShoudBeInvalid()
    {
        DataValidator.IsPasswordValid("1Mp?").Should().BeFalse();
    }

    [Fact]
    public void Password_TooLong_ShoudBeInvalid()
    {
        DataValidator.IsPasswordValid("123456MyLongPassword?;:!").Should().BeFalse();
    }

    [Fact]
    public void Password_Empty_ShoudBeInvalid()
    {
        DataValidator.IsPasswordValid("").Should().BeFalse();
    }

    [Fact]
    public void Password_Null_ShoudBeInvalid()
    {
        DataValidator.IsPasswordValid(null).Should().BeFalse();
    }
}

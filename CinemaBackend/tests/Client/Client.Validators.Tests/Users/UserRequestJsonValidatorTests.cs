using CinemaBackend.Client.Application.UseCases.Users;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using TestsCommonUtilities;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Client.Validators.Tests.Users;

public class UserRequestJsonValidatorTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var request = new UserRequestJsonBuilder()
            .Build();
        context.Add(request);

        var validator = new UserRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
            result.Errors.ShouldBeEmpty();    
        });        
    }

    [Theory]
    [InlineData("")]
    [InlineData("                 ")]
    [InlineData(null)]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters")]
    public async Task Validate_ShouldHaveAnError_WhenNameIsEmpty(string name)
    {
        var context = new TestContext();

        var request = new UserRequestJsonBuilder()
            .WithName(name)
            .Build();
        context.Add(request);

        var validator = new UserRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_NAME_REQUIRED));
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData("                 ")]
    [InlineData(null)]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters")]
    public async Task Validate_ShouldHaveAnError_WhenEmailIsEmpty(string email)
    {
        var context = new TestContext();

        var request = new UserRequestJsonBuilder()
            .WithEmail(email)
            .Build();
        context.Add(request);

        var validator = new UserRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_EMAIL_REQUIRED)); 
        });
    }

    [Fact]
    public async Task Validate_ShouldHaveAnError_WhenEmailIsInvalid()
    {
        var context = new TestContext();

        var request = new UserRequestJsonBuilder()
            .WithEmail("invalid_email")
            .Build();

        context.Add(request);

        var validator = new UserRequestJsonValidator();
        
        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);
            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_EMAIL_INVALID));
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData("                 ")]
    [InlineData(null)]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters")]
    public async Task Validate_ShouldHaveAnError_WhenPasswordIsEmpty(string password)
    {
        var context = new TestContext();
        var request = new UserRequestJsonBuilder()
            .WithPassword(password)
            .Build();
        context.Add(request);

        var validator = new UserRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_PASSWORD_REQUIRED));
        });
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Validate_ShouldHaveAnError_WhenPasswordLengthIsLessThan6(int passwordLength)
    {   
        var context = new TestContext();
        var request = new UserRequestJsonBuilder()
            .WithPasswordLength(passwordLength)
            .Build();
        context.Add(request);

        var validator = new UserRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_PASSWORD_LENGHT));
        });
    }
}

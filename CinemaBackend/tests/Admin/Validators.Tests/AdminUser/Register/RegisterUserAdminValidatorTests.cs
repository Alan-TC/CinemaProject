using System.Diagnostics.CodeAnalysis;
using CinemaBackend.Admin.Application.UseCases.AdminUser.Register;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Admin.Validators.Tests.AdminUser.Register;

public class RegisterUserAdminValidatorTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var request = new RegisterUserAdminRequestJsonBuilder()
            .Build();
        context.Add(request);

        var validator = new RegisterUserAdminValidator();

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
    public async Task Validate_ShouldHaveAnError_WhenEmailIsEmpty(string email)
    {
        var context = new TestContext();

        var request = new RegisterUserAdminRequestJsonBuilder()
            .WithEmail(email)
            .Build();
        context.Add(request);

        var validator = new RegisterUserAdminValidator();

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

        var request = new RegisterUserAdminRequestJsonBuilder()
            .WithEmail("invalid_email")
            .Build();

        context.Add(request);

        var validator = new RegisterUserAdminValidator();
        
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
        var request = new RegisterUserAdminRequestJsonBuilder()
            .WithPassword(password)
            .Build();
        context.Add(request);

        var validator = new RegisterUserAdminValidator();

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
        var request = new RegisterUserAdminRequestJsonBuilder()
            .WithPasswordLength(passwordLength)
            .Build();
        context.Add(request);

        var validator = new RegisterUserAdminValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_PASSWORD_LENGHT));
        });
    }
}

using System.Net;
using CinemaBackend.Admin.Application.UseCases.TheaterLayout.Create;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Repositories;
using TestsCommonUtilities.Repositories.TheaterLayout;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Admin.UseCases.Tests.TheaterLayout.Create;

public class CreateTheaterLayoutUseCaseTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var request = new TheaterLayoutRequestJsonBuilder()
            .Build();
        context.Add(request);

        var useCase = CreateUseCase();
        
        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request);

            result.Tag.ShouldBe(request.Tag);
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenTagExists()
    {
        var context = new TestContext();

        var request = new TheaterLayoutRequestJsonBuilder()
            .Build();
        context.Add(request);

        var useCase = CreateUseCase(request.Tag);
        
        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request)
                .ShouldThrowAsync<ConflictException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.Conflict);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(error => error.Equals(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_TAG_ALREADY_EXISTS));
        });
    }
    private static CreateTheaterLayoutUseCase CreateUseCase(string? tag = null)
    {
        var theaterLayoutReadOnlyRepositoryBuilder = new ITheaterLayoutReadOnlyRepositoryBuilder();

        var theaterLayoutWriteOnlyRepository = new ITheaterLayoutWriteOnlyRepositoryBuilder()
            .Build();

        var unitOfWork = new IUnitOfWorkBuilder()
            .Build();

        if (tag != null)
            theaterLayoutReadOnlyRepositoryBuilder.HasTheaterWithTag(tag);
        
        return new CreateTheaterLayoutUseCase(
            theaterLayoutReadOnlyRepository: theaterLayoutReadOnlyRepositoryBuilder.Build(), 
            theaterLayoutWriteOnlyRepository: theaterLayoutWriteOnlyRepository,
            unitOfWork: unitOfWork);
    }
}

using CinemaBackend.Admin.Application.Mappings;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.Repositories.TheaterLayout;

namespace CinemaBackend.Admin.Application.UseCases.TheaterLayout.Create;

public class CreateTheaterLayoutUseCase(
    ITheaterLayoutReadOnlyRepository theaterLayoutReadOnlyRepository,
    ITheaterLayoutWriteOnlyRepository theaterLayoutWriteOnlyRepository,
    IUnitOfWork unitOfWork
) : ICreateTheaterLayoutUseCase
{
    private readonly ITheaterLayoutReadOnlyRepository _theaterLayoutReadOnlyRepository = theaterLayoutReadOnlyRepository;
    private readonly ITheaterLayoutWriteOnlyRepository _theaterLayoutWriteOnlyRepository = theaterLayoutWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<TheaterLayoutResponseJson> Execute(TheaterLayoutRequestJson request)
    {
        ValidateAndThrowOnError(request);

        var hasTheaterLayout = await _theaterLayoutReadOnlyRepository.HasTheaterWithTag(request.Tag);

        if (hasTheaterLayout)
            throw new ConflictException(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_TAG_ALREADY_EXISTS);
        
        var theaterLayout = request.MapToTheaterLayout();

        await _theaterLayoutWriteOnlyRepository.Add(theaterLayout);
        await _unitOfWork.Commit();

        return theaterLayout.MapToTheaterLayoutResponseJson();
    }

    public static void ValidateAndThrowOnError(TheaterLayoutRequestJson request)
    {
        var validator = new TheaterLayoutRequestJsonValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
            throw new ErrorOnValidationException([.. result.Errors.Select(error => error.ErrorMessage) ]);
    }
}

using CinemaBackend.Admin.Application.Mappings;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.Repositories.TheaterLayout;

namespace CinemaBackend.Admin.Application.UseCases.TheaterLayout.GetById;

public class GetTheaterLayoutByIdUseCase(
    ITheaterLayoutReadOnlyRepository theaterLayoutReadOnlyRepository
) : IGetTheaterLayoutByIdUseCase
{
    private readonly ITheaterLayoutReadOnlyRepository _theaterLayoutReadOnlyRepository = theaterLayoutReadOnlyRepository;

    public async Task<TheaterLayoutResponseJson> Execute(Guid theaterId)
    {
        var theater = await _theaterLayoutReadOnlyRepository.GetById(theaterId);

        if (theater == null)
            throw new NotFoundException();

        return theater.MapToTheaterLayoutResponseJson();
    }
}

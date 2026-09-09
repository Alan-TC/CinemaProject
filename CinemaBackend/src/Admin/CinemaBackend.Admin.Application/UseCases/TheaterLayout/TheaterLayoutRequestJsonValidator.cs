using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.ResourceFiles.Errors;
using FluentValidation;

namespace CinemaBackend.Admin.Application.UseCases.TheaterLayout;

public class TheaterLayoutRequestJsonValidator : AbstractValidator<TheaterLayoutRequestJson>
{
    public TheaterLayoutRequestJsonValidator()
    {
        RuleFor(r => r.Tag)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_TAG_REQUIRED);

        RuleFor(r => r.Layout)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_LAYOUT_REQUIRED);

        RuleFor(r => r.Layout.Width)
            .Must(value => value > 0)
            .WithMessage(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_WIDTH_MIN_VALUE);
        
        RuleFor(r => r.Layout.Height)
            .Must(value => value > 0)
            .WithMessage(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_HEIGHT_MIN_VALUE);
        
        RuleFor(r => r.Layout.Grid)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_GRID_REQUIRED);

        RuleForEach(x => x.Layout.Grid)
            .Must(v => Enum.IsDefined(v.Type))
            .WithMessage(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_INVALID_CELL_TYPE);

        RuleForEach(x => x.Layout.Grid)
            .Must((x, v) => v.PosY >= 0 && v.PosY < x.Layout.Height)
            .WithMessage(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_GRID_POSY_RANGE_VALUE);
        
        RuleForEach(x => x.Layout.Grid)
            .Must((x, v) => v.PosX >= 0 && v.PosX < x.Layout.Width)
            .WithMessage(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_GRID_POSX_RANGE_VALUE);
        
        RuleFor(x => x.Layout.Grid)
            .Must(x => x.DistinctBy(gc => (gc.PosX, gc.PosY)).Count() == x.Count)
            .WithMessage(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_CELL_SAME_POSITION);
    }
}

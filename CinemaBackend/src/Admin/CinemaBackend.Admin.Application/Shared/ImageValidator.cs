using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.SharedKernel.Extensions;
using FluentValidation;

namespace CinemaBackend.Admin.Application.Shared;

internal static class ImageValidator
{
    internal static IRuleBuilderOptions<T, Stream?> Image<T>(this IRuleBuilder<T, Stream?> ruleBuilder)
    {
        return ruleBuilder
            .Must(img => img == null || img.ImageContentType() != null)
            .WithMessage(ResourceErrorMessages.VALIDATION_INVALID_IMAGE);
    }
}

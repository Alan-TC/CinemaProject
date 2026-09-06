using FileTypeChecker.Extensions;
using FileTypeChecker.Types;

namespace CinemaBackend.SharedKernel.Extensions;

public static class StreamExtension
{
    public static string? ImageContentType(this Stream stream)
    {
        if (stream.Length <= 0)
            return null;
            
        string? contentType = null;

        if (stream.Is<JointPhotographicExpertsGroup>())
            contentType = "image/jpeg";
        else if (stream.Is<PortableNetworkGraphic>())
            contentType = "image/png";

        stream.Position = 0;

        return contentType;
    }
}

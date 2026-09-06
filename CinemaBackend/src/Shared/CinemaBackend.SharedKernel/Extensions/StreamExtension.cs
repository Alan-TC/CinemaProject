using FileTypeChecker.Extensions;
using FileTypeChecker.Types;

namespace CinemaBackend.SharedKernel.Extensions;

public static class StreamExtension
{
    public static string? ImageContentType(this Stream stream)
    {
<<<<<<< HEAD
=======
        if (stream.Length <= 0)
            return null;
            
>>>>>>> register-movies
        string? contentType = null;

        if (stream.Is<JointPhotographicExpertsGroup>())
            contentType = "image/png";
        else if (stream.Is<PortableNetworkGraphic>())
            contentType = "image/jpeg";

        stream.Position = 0;

        return contentType;
    }
}

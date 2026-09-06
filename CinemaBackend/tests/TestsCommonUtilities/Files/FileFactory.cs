namespace TestsCommonUtilities.Files;

public static class FileFactory
{
    private const string FILE_TXT = "file.txt";
    private const string FILE_BMP = "image.bmp";
    private const string FILE_PNG = "image.png";
    private const string FILE_JPEG = "image.jpg";

    public static Stream GetTXT() => GetStream(FILE_TXT);

    public static Stream GetBMP() => GetStream(FILE_BMP);

    public static Stream GetPNG() => GetStream(FILE_PNG);

    public static Stream GetJPEG() => GetStream(FILE_JPEG);

    private static Stream GetStream(string fileName)
    {
        var assembly = typeof(FileFactory).Assembly;

        var resourceName = assembly.GetManifestResourceNames()
            .Single(name => name.EndsWith(fileName));

        return assembly.GetManifestResourceStream(resourceName)!;
    }
}

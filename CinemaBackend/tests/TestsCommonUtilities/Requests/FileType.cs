using TestsCommonUtilities.Files;

namespace TestsCommonUtilities.Requests;

public static class FileType
{
    public const string PNG = "png";
    public const string JPEG = "jpeg";
    public const string BMP = "bmp";
    public const string TXT = "txt";

    public static Stream? GetStream(string fileType)
    {
        return fileType switch
        {
            FileType.PNG => FileFactory.GetPNG(),
            FileType.JPEG => FileFactory.GetJPEG(),
            FileType.BMP => FileFactory.GetBMP(),
            FileType.TXT => FileFactory.GetTXT(),
            _ => null
        };
    }
}
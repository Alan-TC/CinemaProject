using System.Security.Cryptography;

namespace CinemaBackend.SharedKernel.Services;

public static class CodeGenerator
{
    public static string GenerateCode(int length) =>
        RandomNumberGenerator.GetString("0123456789BCDFGHJKLMNPQRSTVWXZ", length);
}

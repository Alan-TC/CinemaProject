using System.Security.Cryptography;
using System.Text;
using CinemaBackend.Domain.Security.PasswordHasher;
using Konscious.Security.Cryptography;

namespace CinemaBackend.Infrastructure.Security.PasswordHasher;

public class PasswordHasherArgon2id : IPasswordHasher
{
    private const int PASSWORD_BYTES_SIZE = 32;
    private const int SALT_SIZE = 16;
    private const int DEGREE_OF_PARALLELISM = 1;
    private const int INTERACTIONS = 2;
    private const int MEMORY_SIZE = 19 * 1024;

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SALT_SIZE);

        var passwordHash = HashPassword(password, salt);

        var combinedHash = new byte[salt.Length + passwordHash.Length];

        salt.CopyTo(combinedHash);
        passwordHash.CopyTo(combinedHash, salt.Length);

        return Convert.ToBase64String(combinedHash);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var combinedHashBytes = Convert.FromBase64String(hashedPassword);

        var salt = combinedHashBytes[..SALT_SIZE];
        var passwordHash = combinedHashBytes[SALT_SIZE..];

        var toVerifyPasswordHash = HashPassword(password, salt);

        return CryptographicOperations.FixedTimeEquals(passwordHash, toVerifyPasswordHash);
    }

    private static byte[] HashPassword(string password, byte[] salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        var argon = new Argon2id(passwordBytes)
        {
            Salt = salt,
            DegreeOfParallelism = DEGREE_OF_PARALLELISM,
            Iterations = INTERACTIONS,
            MemorySize = MEMORY_SIZE
        };

        var passwordHash = argon.GetBytes(PASSWORD_BYTES_SIZE);

        return passwordHash;
    }
}

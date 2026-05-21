using System.Text;
using System.Security.Cryptography;

namespace JobPortalAPI.API.Helpers;

public class PasswordHasher
{
    public static string Hash(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public static bool Verify(string password, string hash)
    {
        return Hash(password) == hash;
    }

}

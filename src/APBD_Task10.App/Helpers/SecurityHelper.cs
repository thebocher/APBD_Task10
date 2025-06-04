using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace APBD_Task10.App.Helpers;

public static class SecurityHelper
{
    private static byte[] _salt = new byte[128 / 8];
    
    public static string GetHashedPassword(string password)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: _salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 1000,
            numBytesRequested: 256 / 8
            ));
    }
}
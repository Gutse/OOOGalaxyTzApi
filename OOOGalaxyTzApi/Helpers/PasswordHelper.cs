using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using OOOGalaxyTzApi.Interfaces;
using OOOGalaxyTzApi.Models;

namespace OOOGalaxyTzApi.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {
        public bool VerifyHashedPassword(UserModel user, string providedPassword)
        {
            if (user == null || string.IsNullOrWhiteSpace(providedPassword))
                return false;

            var providedHashed = HashPassword(providedPassword, Convert.FromBase64String(user.Salt));
            return string.Equals(user.Password, providedHashed);
        }

        public (string hash, string salt) GetNewPassword(string input)
        {
            var salt = GenerateSalt();
            var hashed = HashPassword(input, salt);
            return (hashed, Convert.ToBase64String(salt));
        }

        private static string HashPassword(string input, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8));
        }

        private static byte[] GenerateSalt()
        {
            var salt = new byte[128 / 8];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }
    }
}

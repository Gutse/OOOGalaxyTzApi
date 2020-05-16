using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace OOOGalaxyTzApi.Auth
{
    /// <summary>
    /// Настройки генерации токенов
    /// </summary>
    public static class AuthOptions
    {
        /// <summary>
        /// Издатель токена
        /// </summary>
        public const string Issuer = "OOOGalaxyTzApi";

        /// <summary>
        /// Потребитель токена
        /// </summary>
        public const string Audience = null;

        /// <summary>
        /// Время жизни токена
        /// </summary>
        public static DateTime? Lifetime;

        /// <summary>
        /// Ключ
        /// </summary>
        private const string Key = "SJWUNMNKDFFKNDhxcuyKLlknfdsmnweNKMNSdmnmnerKMNMSNdmnasdywermnzJHCmnzc";

        /// <summary>
        /// GetKey
        /// </summary>
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}
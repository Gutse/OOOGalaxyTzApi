using OOOGalaxyTzApi.Models;

namespace OOOGalaxyTzApi.Interfaces
{
    public interface IPasswordHelper
    {
        (string hash, string salt) GetNewPassword(string input);

        bool VerifyHashedPassword(UserModel user, string providedPassword);
    }
}

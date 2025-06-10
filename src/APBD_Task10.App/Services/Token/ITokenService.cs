using APBD_Task10.Infrastructure;

namespace APBD_Task10.App.Services;

public interface ITokenService
{
    string GenerateToken(string email, string role);
}
using APBD_Task10.App.DTOs.Account;

namespace APBD_Task10.App.Services;

public interface IAccountService
{
    public void Register(RegisterRequestDto dto);
}
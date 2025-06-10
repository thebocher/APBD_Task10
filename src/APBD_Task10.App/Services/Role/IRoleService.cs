using APBD_Task10.App.DTOs;
using APBD_Task10.App.DTOs.Role;

namespace APBD_Task10.App.Services.Role;

public interface IRoleService
{
    List<GetRoleListItemDto> GetRoles();
}
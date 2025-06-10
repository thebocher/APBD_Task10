using APBD_Task10.App.DTOs.Role;
using APBD_Task10.App.Services.Role;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Task10.API.Controllers;

[ApiController]
[Route("/api/roles")]
public class RoleController
{
    private IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public List<GetRoleListItemDto> getRoles()
    {
        return _roleService.GetRoles();
    }
}
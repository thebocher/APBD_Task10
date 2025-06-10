using APBD_Task10.App.DTOs.Role;
using APBD_Task10.Infrastructure.DAL;

namespace APBD_Task10.App.Services.Role;

public class RoleService : IRoleService
{
    private MasterContext _context;

    public RoleService(MasterContext context)
    {
        _context = context;
    }
    
    public List<GetRoleListItemDto> GetRoles()
    {
        return _context.Role.Select(r => new GetRoleListItemDto()
        {
            Id = r.Id,
            Name = r.Name,
        }).ToList();
    }
}
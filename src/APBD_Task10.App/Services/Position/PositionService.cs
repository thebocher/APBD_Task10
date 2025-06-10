using APBD_Task10.App.DTOs;
using APBD_Task10.Infrastructure.DAL;

namespace APBD_Task10.App.Services.Position;

public class PositionService : IPositionService
{
    private MasterContext _context;

    public PositionService(MasterContext context)
    {
        _context = context;
    }
    
    public List<GetPositionListItemDto> GetPositions()
    {
        return _context.Position.Select(p => new GetPositionListItemDto()
        {
            Id = p.Id,
            Name = p.Name,
        }).ToList();
    }
}
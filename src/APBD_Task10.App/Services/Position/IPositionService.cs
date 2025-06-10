using APBD_Task10.App.DTOs;

namespace APBD_Task10.App.Services.Position;

public interface IPositionService
{
    public List<GetPositionListItemDto> GetPositions();
}
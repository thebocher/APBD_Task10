using APBD_Task10.App.DTOs;
using APBD_Task10.App.Services.Position;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Task10.API.Controllers;

[ApiController]
[Route("/api/positions")]
public class PositionController
{
    private IPositionService _positionService;

    public PositionController(IPositionService positionService)
    {
        _positionService = positionService;
    }

    [HttpGet]
    public List<GetPositionListItemDto> GetPositions()
    {
        return _positionService.GetPositions();
    }
}
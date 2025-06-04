using APBD_Task10.App.DTOs.Account;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Task10.API.Controllers;

[ApiController]
[Route("/api/accounts")]
public class AccountController
{
    [HttpPost]
    public IResult register([FromBody] RegisterRequestDto dto)
    {
        return Results.Ok();
    }
    
}
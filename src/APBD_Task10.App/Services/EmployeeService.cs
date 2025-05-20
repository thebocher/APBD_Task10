using APBD_Task10.App.DTOs;
using APBD_Task10.Infrastructure;
using APBD_Task10.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace APBD_Task10.App.Services;

public class EmployeeService(MasterContext context) : IEmployeeService
{
    private MasterContext _context = context;
    
    public List<GetEmployeeListItemDto> GetEmployees()
    {
        return _context.Employees
            .Include(e => e.Person)
            .Select(e => new GetEmployeeListItemDto
            {
                Id = e.Id,
                Name = e.Person.GetFullName(),
            })
            .ToList();
    }

    public GetEmployeeDto? GetEmployee(int id)
    {
        return _context.Employees
            .Include(e => e.Person)
            .Include(e => e.Position)
            .Where(e => e.Id == id)
            .Select(e => new GetEmployeeDto
            {
                Person = e.Person,
                Salary = e.Salary,
                Position = new PositionDto
                {
                    Id = e.PositionId,
                    Name = e.Position.Name,

                },
                HireDate = e.HireDate,
            })
            .FirstOrDefault();
    }
}
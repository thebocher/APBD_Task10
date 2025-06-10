using APBD_Task10.App.DTOs;
using APBD_Task10.App.DTOs.Employee;
using APBD_Task10.Infrastructure;
using APBD_Task10.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace APBD_Task10.App.Services;

public class EmployeeService(MasterContext context) : IEmployeeService
{
    private MasterContext _context = context;
    
    public List<GetEmployeeListItemDto> GetEmployees()
    {
        return _context.Employee
            .Include(e => e.Person)
            .Select(e => new GetEmployeeListItemDto
            {
                Id = e.Id,
                FullName = e.Person.GetFullName(),
            })
            .ToList();
    }

    public GetEmployeeDto? GetEmployee(int id)
    {
        return _context.Employee
            .Include(e => e.Person)
            .Include(e => e.Position)
            .Where(e => e.Id == id)
            .Select(e => new GetEmployeeDto
            {
                Person = new GetEmployeePersonDto()
                {   
                    Id = e.Person.Id,
                    PassportNumber = e.Person.PassportNumber,
                    FirstName = e.Person.FirstName,
                    MiddleName = e.Person.MiddleName,
                    LastName = e.Person.LastName,
                    PhoneNumber = e.Person.PhoneNumber,
                    Email = e.Person.Email,
                },
                Salary = e.Salary,
                Position = e.Position.Name,
                HireDate = e.HireDate,
            })
            .FirstOrDefault();
    }

    public GetEmployeeDto CreateEmployee(CreateEmployeeDto dto)
    {
        var person = new Person()
        {
            PassportNumber = dto.Person.PassportNumber,
            FirstName = dto.Person.FirstName,
            MiddleName = dto.Person.MiddleName,
            LastName = dto.Person.LastName,
            Email = dto.Person.Email,
            PhoneNumber = dto.Person.PhoneNumber,
        };
        _context.Person.Add(person);

        var employee = new Employee()
        {
            Person = person,
            Salary = dto.Salary,
            PositionId = dto.PositionId,
        };
        _context.Employee.Add(employee);
        _context.SaveChanges();

        return new GetEmployeeDto()
        {
            HireDate = employee.HireDate,
            Salary = employee.Salary,
            Position = employee.Position.Name,
            Person = new GetEmployeePersonDto()
            {
                Id = employee.Person.Id,
                Email = employee.Person.Email,
                PassportNumber = employee.Person.PassportNumber,
                FirstName = employee.Person.FirstName,
                MiddleName = employee.Person.MiddleName,
                LastName = employee.Person.LastName,
                PhoneNumber = employee.Person.PhoneNumber,
            }
        };
    }
}
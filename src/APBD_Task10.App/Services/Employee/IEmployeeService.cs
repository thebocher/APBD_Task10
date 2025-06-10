using APBD_Task10.App.DTOs;
using APBD_Task10.App.DTOs.Employee;

namespace APBD_Task10.App.Services;

public interface IEmployeeService
{
    public List<GetEmployeeListItemDto> GetEmployees();
    public GetEmployeeDto? GetEmployee(int id);
    public GetEmployeeDto CreateEmployee(CreateEmployeeDto dto);
}
using APBD_Task10.App.DTOs;

namespace APBD_Task10.App.Services;

public interface IEmployeeService
{
    public List<GetEmployeeListItemDto> GetEmployees();
    public GetEmployeeDto? GetEmployee(int id);
}
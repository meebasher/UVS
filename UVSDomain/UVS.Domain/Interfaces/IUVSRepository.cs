using UVS.Domain.Entities;

namespace UVS.Domain.Interfaces
{
    public interface IUVSRepository
    {
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<Employee> CreateOrUpdateEmployeeAsync(Employee employee);
    }
}

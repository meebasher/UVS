using MediatR;
using UVS.Contracts.Employee;
namespace UVS.Application.Commands
{
    public record EmployeeSetCommand(int employeeId, string employeeName, int employeeSalary) : IRequest<EmployeeResponse>;
}

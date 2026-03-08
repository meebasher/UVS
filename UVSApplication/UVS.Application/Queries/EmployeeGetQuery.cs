using MediatR;
using UVS.Contracts.Employee;

namespace UVS.Application.Queries
{
    public record EmployeeGetQuery(int employeeId) : IRequest<EmployeeResponse>;
}

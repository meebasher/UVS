using MediatR;
using UVS.Contracts.Employee;
using UVS.Domain.Entities;
using UVS.Domain.Interfaces;

namespace UVS.Application.Commands.Handlers
{
    public class EmployeeSetCommandHandler : IRequestHandler<EmployeeSetCommand, EmployeeResponse>
    {
        private readonly IUVSRepository _uvsRepository;

        public EmployeeSetCommandHandler(IUVSRepository uvsRepository)
        {
            _uvsRepository = uvsRepository;
        }

        public async Task<EmployeeResponse> Handle(EmployeeSetCommand command, CancellationToken cancellationToken)
        {
            var employee = new Employee { Id = command.employeeId, Name = command.employeeName, Salary = command.employeeSalary };
            var employeeFromRepo = await _uvsRepository.CreateOrUpdateEmployeeAsync(employee);
            var emplyeeToReturn = new EmployeeResponse(employeeFromRepo.Name, employeeFromRepo.Salary);
            return emplyeeToReturn;
        }
    }
}

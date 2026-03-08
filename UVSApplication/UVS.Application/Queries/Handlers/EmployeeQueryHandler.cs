using MediatR;
using UVS.Contracts.Employee;
using UVS.Domain.Interfaces;

namespace UVS.Application.Queries.Handlers
{
    public class EmployeeQueryHandler : IRequestHandler<EmployeeGetQuery, EmployeeResponse?>
    {
        private readonly IUVSRepository _uvsRepository;

        public EmployeeQueryHandler(IUVSRepository uvsRepository)
        {
            _uvsRepository = uvsRepository ?? throw new ArgumentNullException(nameof(uvsRepository));
        }

        public async Task<EmployeeResponse?> Handle(EmployeeGetQuery request, CancellationToken cancellationToken)
        {
            var employeeFromRepo = await _uvsRepository.GetEmployeeByIdAsync(request.employeeId);

            return employeeFromRepo == null ? null : new EmployeeResponse(employeeFromRepo.Name, employeeFromRepo.Salary);
        }
    }
}

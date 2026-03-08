using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UVS.Domain.Entities;
using UVS.Domain.Interfaces;
using UVS.Infra.Data.Context;

namespace UVS.Infra.Data.Repository
{
    public class UVSRepository : IUVSRepository
    {
        private readonly UVSDbContext _context;
        private readonly ILogger<UVSRepository> _logger;

        public UVSRepository(UVSDbContext context, ILogger<UVSRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            try
            {
                var doesEmployeeExist = await EmployeeExistsAsync(employeeId);

                if (!doesEmployeeExist)
                {
                    _logger.LogWarning("A trie to retrieve non existing employee with ID {EmployeeId}", employeeId);
                    return null;
                }


                return await _context.Employees
                    .AsNoTracking()
                    .SingleOrDefaultAsync(e => e.Id == employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee with ID {EmployeeId}", employeeId);
                throw;
            }
        }

        public async Task<Employee> CreateOrUpdateEmployeeAsync(Employee employee)
        {
            var exists = await EmployeeExistsAsync(employee.Id);

            if (exists)
            {
                await UpdateEmployeeAsync(employee);
            }
            else
            {
                await CreateEmployeeAsync(employee);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //log.error
                throw;
            }

            var createdOrUpdatedEmployee = await GetEmployeeByIdAsync(employee.Id);

            return createdOrUpdatedEmployee!;
        }

        private async Task<bool> EmployeeExistsAsync(int employeeId)
        {
            try
            {
                return await _context.Employees.AsNoTracking().AnyAsync(
               p => p.Id == employeeId) == true ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee with ID {EmployeeId}", employeeId);
                throw;
            }

        }

        private async Task CreateEmployeeAsync(Employee employee)
        {
            try
            {
                await _context.Employees.AddAsync(employee);
                _logger.LogInformation("Created new employee with ID {EmployeeId}", employee.Id);
            }
            catch (Exception ex)
            {
                //log.error
                throw;
            }

        }

        private async Task UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                var existing = await _context.Employees.FindAsync(employee.Id);
                if (existing == null)
                {
                    return;
                }

                existing.Name = employee.Name;
                existing.Salary = employee.Salary;
                _context.Employees.Update(existing);
                _logger.LogInformation("Updated employee with ID {EmployeeId}", employee.Id);
            }
            catch (Exception ex)
            {
                //log.error
                throw;
            }
        }
    }
}

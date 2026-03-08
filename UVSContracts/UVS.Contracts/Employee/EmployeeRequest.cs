namespace UVS.Contracts.Employee
{
    public record EmployeeRequest(int employeeId, string? employeeName = null, string? employeeSalary = null);
}

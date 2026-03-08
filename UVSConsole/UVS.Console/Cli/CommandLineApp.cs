using MediatR;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using UVS.Application.Commands;
using UVS.Application.Queries;

namespace UVSConsoleApp.Cli
{
    public class CommandLineApp
    {
        private readonly IMediator _mediator;

        public CommandLineApp(IMediator mediator, ILogger<CommandLineApp> logger)
        {
            _mediator = mediator;
        }

        public async Task<int> RunAsync(string[] args)
        {
            var root = new RootCommand("Employee Management Console Application");

            // Get employee
            var getEmployeeIdOption = new Option<int>("--employeeId", "The ID of the employee to retrieve") { IsRequired = true };
            var get = new Command("get-employee", "Retrieve employee information by ID");
            get.AddOption(getEmployeeIdOption);
            get.SetHandler(async (int employeeId) =>
            {
                var response = await _mediator.Send(new EmployeeGetQuery(employeeId));
                if (response == null)
                {
                    System.Console.WriteLine($"Employee with id {employeeId} not found");
                }
                else
                {
                    System.Console.WriteLine($"Id: {employeeId}, Name: {response.employeeName}, Salary: {response.employeeSalary}");
                }
            }, getEmployeeIdOption);

            var setEmployeeIdOption = new Option<int>("--employeeId", "The ID of the employee") { IsRequired = true };
            var setEmployeeNameOption = new Option<string>("--employeeName", "The name of the employee") { IsRequired = true };
            var setEmployeeSalaryOption = new Option<int>("--employeeSalary", "The salary of the employee") { IsRequired = true };
            var set = new Command("set-employee", "Create or update employee information");
            set.AddOption(setEmployeeIdOption);
            set.AddOption(setEmployeeNameOption);
            set.AddOption(setEmployeeSalaryOption);

            set.SetHandler(async (int employeeId, string employeeName, int employeeSalary) =>
            {
                var response = await _mediator.Send(new EmployeeSetCommand(employeeId, employeeName, employeeSalary));
                Console.WriteLine($"Saved Employee: Name={response.employeeName}, Salary={response.employeeSalary}");
            }, setEmployeeIdOption, setEmployeeNameOption, setEmployeeSalaryOption);

            root.AddCommand(get);
            root.AddCommand(set);

            return await root.InvokeAsync(args);
        }
    }
}

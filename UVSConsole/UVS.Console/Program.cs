using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using UVS.Infra.Data.Context;
using UVS.IoC;
using UVSConsoleApp.Cli;

namespace UVSConsoleApp
{
    public class Program
    {
        static async Task<int> Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            var services = new ServiceCollection();
            services.AddUVSServices(configuration);
            services.AddLogging(builder => builder.AddConsole());
            services.AddTransient<CommandLineApp>();

            using var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            await serviceProvider.MigrateDbContextGenericAsync<UVSDbContext>();


            RootCommand rootCommand = BuildCommands();

            if (args.Length == 0)
            {
                while (true)
                {
                    Console.Write("Enter command: ");
                    string? input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input)) continue;
                    var inputArgs = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var parseResult = rootCommand.Parse(inputArgs);
                    if (parseResult.Errors.Any())
                    {
                        foreach (var error in parseResult.Errors)
                            Console.WriteLine(error.Message);
                        continue;
                    }
                    var invoked = parseResult.CommandResult.Command?.Name ?? string.Empty;
                    if (!string.IsNullOrEmpty(invoked))
                    {
                        using var scope = serviceProvider.CreateScope();
                        var scopedProvider = scope.ServiceProvider;
                        var cli = scopedProvider.GetRequiredService<CommandLineApp>();
                        await cli.RunAsync(inputArgs);
                    }
                }
            }
            else
            {
                var parseResult = rootCommand.Parse(args);
                var invoked = parseResult.CommandResult.Command?.Name ?? string.Empty;
                using var scope = serviceProvider.CreateScope();
                var scopedProvider = scope.ServiceProvider;
                var cli = scopedProvider.GetRequiredService<CommandLineApp>();
                return await cli.RunAsync(args);
            }
        }

        private static RootCommand BuildCommands()
        {
            var rootCommand = new RootCommand("UVS Application");

            var getEmployeeIdOption = new Option<int>("--employeeId", "The ID of the employee to retrieve");
            var getCommand = new Command("get-employee", "Retrieve employee information by ID") { getEmployeeIdOption };

            var setEmployeeIdOption = new Option<int>("--employeeId", "The ID of the employee");
            var setEmployeeNameOption = new Option<string>("--employeeName", "The name of the employee");
            var setEmployeeSalaryOption = new Option<int>("--employeeSalary", "The salary of the employee");
            var setCommand = new Command("set-employee", "Create or update employee information") { setEmployeeIdOption, setEmployeeNameOption, setEmployeeSalaryOption };

            rootCommand.AddCommand(getCommand);
            rootCommand.AddCommand(setCommand);
            return rootCommand;
        }

    }
}

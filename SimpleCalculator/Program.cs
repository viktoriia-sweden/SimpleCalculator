using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SimpleCalculator.Infrastructure.Processors;
using SimpleCalculator.Infrastructure.Repositories;
using SimpleCalculator.Infrastructure.Services;
using SimpleCalculator.Infrastructure.Validators;

namespace SimpleCalculator
{
	internal class Program
	{
		static void Main(string[] args)
		{
			HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

			builder.Services.AddSingleton<ICommandResolver, CommandResolver>();
			builder.Services.AddSingleton<ICommandValidator, CommandValidator>();
			builder.Services.AddSingleton<ICalculatorService, CalculatorService>();
			builder.Services.AddSingleton<IConsoleService, ConsoleService>();
			builder.Services.AddSingleton<ICalculatorProcessor, CalculatorProcessor>();
			builder.Services.AddSingleton<IRegisterRepository, RegisterRepository>();

			using IHost host = builder.Build();

			var service = host.Services.GetRequiredService<ICalculatorService>();
			service.Run(args);
		}
	}
}
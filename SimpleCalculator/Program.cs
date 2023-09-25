using Microsoft.Extensions.DependencyInjection;
using SimpleCalculator.Infrastructure.Repositories;
using SimpleCalculator.Infrastructure.Services;
using SimpleCalculator.Infrastructure.Validators;

namespace SimpleCalculator
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var serviceProvider = new ServiceCollection()
				.AddSingleton<IRegisterRepository, RegisterRepository>()
				.AddSingleton<ICommandResolver, CommandResolver>()
				.AddSingleton<ICommandValidator, CommandValidator>()
				.AddSingleton<ICommandService, CommandService>()
				.BuildServiceProvider();

			var commandService = serviceProvider.GetService<ICommandService>();
			commandService!.Run(args);
		}
	}
}
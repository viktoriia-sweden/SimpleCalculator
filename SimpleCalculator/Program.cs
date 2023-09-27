﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
			var serviceProvider = new ServiceCollection()
				.AddSingleton<IRegisterRepository, RegisterRepository>()
				.AddSingleton<ICommandResolver, CommandResolver>()
				.AddSingleton<ICommandValidator, CommandValidator>()
				.AddSingleton<ICalculatorService, CalculatorService>()
				.AddSingleton<ICalculatorProcessor, CalculatorProcessor>()
				.AddSingleton<IConsoleService, ConsoleService>()
				.AddLogging((loggingBuilder) => loggingBuilder
					.SetMinimumLevel(LogLevel.Information)
					.AddConsole())
				.BuildServiceProvider();

			var commandService = serviceProvider.GetService<ICalculatorService>();
			commandService!.Run(args);
		}
	}
}
using Microsoft.Extensions.Logging;

using SimpleCalculator.Infrastructure.Services;
using SimpleCalculator.Infrastructure.Validators;

namespace SimpleCalculator.Infrastructure.Processors
{
	public class CalculatorProcessor : ICalculatorProcessor
	{
		public CalculatorProcessor
		(
			ICommandValidator commandValidator,
			ICommandResolver commandResolver,
			IConsoleService consoleService,
			ILogger<CalculatorProcessor> logger
		)
		{
			_commandValidator = commandValidator;
			_commandResolver = commandResolver;
			_consoleService = consoleService;
			_logger = logger;
		}

		public void ProcessConsole()
		{
			_logger.LogInformation($"Program is opened in \"Console\" mode.");
			_logger.LogInformation("Please enter a command.");

			while (!_commandResolver.IsQuit)
			{
				Process(_consoleService.Read());
			}

			_logger.LogInformation("Command \"quit\" was called. Exit.");
		}

		public void ProcessFile(string fileName)
		{
			_logger.LogInformation($"Program is opened in \"File\" mode.");
			_logger.LogInformation("Reading from file...");

			using var sr = new StreamReader(fileName);

			string? line;
			while (!_commandResolver.IsQuit && (line = sr.ReadLine()) != null)
			{
				Process(line);
			}

			_logger.LogInformation("The file was read or command \"quit\" was called. Exit.");
		}

		private void Process(string? line)
		{
			var command = GetCommand(line);

			if (_commandValidator.IsValid(command))
			{
				var process = _commandResolver.Process(command!);
				process.Process(command!);
			}
		}

		private static string[]? GetCommand(string? str) => str?.Trim().Trim('\n').ToLower().Split(" ");

		private readonly ILogger<CalculatorProcessor> _logger;
		private readonly ICommandValidator _commandValidator;
		private readonly ICommandResolver _commandResolver;
		private readonly IConsoleService _consoleService;
	}
}

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

		/// <summary>
		/// Reads commands and calls validators and processors to evaluate them.
		/// </summary>
		public void ProcessConsole()
		{
			_logger.LogInformation($"Program is opened in \"Console\" mode.");
			_logger.LogInformation("Please enter a command.");

			while (Process(_consoleService.Read()))
			{
			}

			_logger.LogInformation("Exit.");
		}

		/// <summary>
		/// Reads commands from file and calls validators and processors to evaluate them.
		/// </summary>
		/// <param name="file">file</param>
		public void ProcessFile(string file)
		{
			_logger.LogInformation($"Program is opened in \"File\" mode.");
			_logger.LogInformation("Reading from file...");

			using var sr = new StreamReader(file);

			string? line;
			while ((line = sr.ReadLine()) != null && Process(line))
			{
			}

			_logger.LogInformation("Exit.");
		}

		/// <summary>
		/// Calls validators and processors to evaluate commands. 
		/// </summary>
		/// <param name="line">Read line.</param>
		/// <returns>Should process be continued.</returns>
		private bool Process(string? line)
		{
			var command = GetCommand(line);

			if (_commandValidator.IsValid(command))
			{
				var processor = _commandResolver.Process(command!);
				try
				{
					processor.Process(command!);
					return !_commandResolver.IsQuit;
				}
				catch (Exception ex)
				{
					_logger.LogCritical(ex.Message);
					return false;
				}
			}

			return true;
		}

		private static string[]? GetCommand(string? str) => str?.Trim().Trim('\n').ToLower().Split(" ");

		private readonly ILogger<CalculatorProcessor> _logger;
		private readonly ICommandValidator _commandValidator;
		private readonly ICommandResolver _commandResolver;
		private readonly IConsoleService _consoleService;
	}
}

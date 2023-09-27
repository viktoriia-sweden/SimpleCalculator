using SimpleCalculator.Infrastructure.Services;
using SimpleCalculator.Infrastructure.Validators;

namespace SimpleCalculator.Infrastructure.Processors
{
	public class CalculatorProcessor : ICalculatorProcessor
	{
		public CalculatorProcessor
		(
			ICommandValidator commandValidator,
			ICommandResolver commandResolver
		)
		{
			_commandValidator = commandValidator;
			_commandResolver = commandResolver;
		}

		public void ProcessConsole()
		{
			Console.WriteLine($"[Information]. Program is opened in \"Console\" mode.");
			Console.WriteLine("[Information]. Please enter a command.");

			while (!_commandResolver.IsQuit)
			{
				Process(Console.ReadLine());
			}

			Console.WriteLine("[Information]. Command \"quit\" was called. Exit.");
		}

		public void ProcessFile(string fileName)
		{
			Console.WriteLine($"[Information]. Program is opened in \"File\" mode.");
			Console.WriteLine("[Information]. Reading from file...");

			using var sr = new StreamReader(fileName);

			string? line;
			while (!_commandResolver.IsQuit && (line = sr.ReadLine()) != null)
			{
				Process(line);
			}

			Console.WriteLine("[Information]. The file was read or command \"quit\" was called. Exit.");
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

		private readonly ICommandValidator _commandValidator;
		private readonly ICommandResolver _commandResolver;
	}
}

using SimpleCalculator.Infrastructure.Services;
using SimpleCalculator.Infrastructure.Validators;

namespace SimpleCalculator.Infrastructure.Processors
{
	public class CommandProcessor : ICommandProcessor
	{
		public CommandProcessor
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
			while (!_commandResolver.IsQuit)
			{
				var command = GetCommand(Console.ReadLine());
				Process(command);
			}
		}

		public void ProcessFile(string fileName)
		{
			using var sr = new StreamReader(fileName);

			string? line;
			while (!_commandResolver.IsQuit && (line = sr.ReadLine()) != null)
			{
				var command = GetCommand(line);
				Process(command);
			}
		}

		private void Process(string[]? command)
		{
			if (_commandValidator.IsValid(command))
			{
				_commandResolver.Process(command!);
			}
		}
		private static string[]? GetCommand(string? str) => str?.Trim().Trim('\n').ToLower().Split(" ");


		private readonly ICommandValidator _commandValidator;
		private readonly ICommandResolver _commandResolver;
	}
}

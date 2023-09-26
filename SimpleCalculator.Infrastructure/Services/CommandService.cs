using SimpleCalculator.Infrastructure.Validators;

namespace SimpleCalculator.Infrastructure.Services
{
	public class CommandService : ICommandService
	{
		public CommandService
		(
			ICommandValidator commandValidator,
			ICommandResolver commandProcessor
		)
		{
			_commandValidator = commandValidator;
			_commandProcessor = commandProcessor;
		}

		public void Run(string[] args)
		{
			if (args.Length > 0)
			{
				using var sr = new StreamReader(args[0]);
				var command = GetCommand(sr.ReadLine());
				while (!_commandProcessor.IsQuit() && command != null)
				{
					Process(command);
					command = GetCommand(sr.ReadLine());
				}
			}
			else
			{
				while (!_commandProcessor.IsQuit())
				{
					var command = GetCommand(Console.ReadLine());
					Process(command);
				}
			}
		}

		private void Process(string[]? command)
		{
			if (_commandValidator.IsValid(command))
			{
				_commandProcessor.Process(command!);
			}
		}
		private static string[]? GetCommand(string? str) => str?.Trim().Trim('\n').ToLower().Split(" ");

		private readonly ICommandValidator _commandValidator;
		private readonly ICommandResolver _commandProcessor;
	}
}

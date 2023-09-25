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
				var fileName = args[0];

				using var sr = new StreamReader(fileName);
				var line = sr.ReadLine();
				while (line != null)
				{
					var command = line?.Trim().Trim('\n').ToLower().Split(" ");

					Process(command);
					if (_commandProcessor.IsQuit) break;

					line = sr.ReadLine();
				}
			}
			else
			{
				while (true)
				{
					var input = Console.ReadLine();
					var command = input?.Trim().ToLower().Split(" ");

					Process(command);
					if (_commandProcessor.IsQuit) break;
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

		private readonly ICommandValidator _commandValidator;
		private readonly ICommandResolver _commandProcessor;
	}
}

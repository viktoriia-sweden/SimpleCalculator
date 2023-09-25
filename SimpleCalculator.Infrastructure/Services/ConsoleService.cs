using SimpleCalculator.Infrastructure.Processors;
using SimpleCalculator.Infrastructure.Validators;

namespace SimpleCalculator.Infrastructure.Services
{
	public class ConsoleService : ICommandService
	{
		public ConsoleService(ICommandValidator commandValidator, ICommandResolver commandProcessor)
		{
			_commandValidator = commandValidator;
			_commandProcessor = commandProcessor;
		}

		public void Run()
		{
			while (true)
			{
				var input = Console.ReadLine();
				var command = input?.Trim().ToLower().Split(" ");

				if (_commandValidator.IsValid(command))
				{
					_commandProcessor.Process(command!);
					if (_commandProcessor.IsQuit) break;
				}
			}
		}

		private readonly ICommandValidator _commandValidator;
		private readonly ICommandResolver _commandProcessor;
	}
}

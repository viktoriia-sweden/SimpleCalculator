using SimpleCalculator.Infrastructure.Handlers;
using SimpleCalculator.Infrastructure.Services;

namespace SimpleCalculator
{
	public class ConsoleService : ICommandService
	{
		public ConsoleService(ICommandProcessor commandProcessor)
		{
			_commandProcessor = commandProcessor;
		}

		public void Run()
		{
			while (true)
			{
				var input = Console.ReadLine();

				if (string.IsNullOrEmpty(input))
				{
					Console.WriteLine("Please enter a valid command.");
				}

				var command = input!.Trim().ToLower().Split(" ");

				_commandProcessor.Process(command);
			}
		}

		private readonly ICommandProcessor _commandProcessor;
	}
}

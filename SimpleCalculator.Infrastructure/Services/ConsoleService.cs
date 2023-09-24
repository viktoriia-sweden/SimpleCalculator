using SimpleCalculator.Infrastructure.Processors;

namespace SimpleCalculator.Infrastructure.Services
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
				var command = input!.Trim().ToLower().Split(" ");

				_commandProcessor.Process(command);
			}
		}

		private readonly ICommandProcessor _commandProcessor;
	}
}

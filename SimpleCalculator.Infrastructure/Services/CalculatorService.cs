using SimpleCalculator.Infrastructure.Processors;

namespace SimpleCalculator.Infrastructure.Services
{
	public class CalculatorService : ICalculatorService
	{
		public CalculatorService(ICommandProcessor commandProcessor)
		{
			_commandProcessor = commandProcessor;
		}

		public void Run(string[] args)
		{
			if (args.Length > 0)
			{
				_commandProcessor.ProcessFile(args[0]);
			}
			else
			{
				_commandProcessor.ProcessConsole();
			}
		}

		private readonly ICommandProcessor _commandProcessor;
	}
}

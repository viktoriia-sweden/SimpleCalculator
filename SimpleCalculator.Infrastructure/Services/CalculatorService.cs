using SimpleCalculator.Infrastructure.Processors;

namespace SimpleCalculator.Infrastructure.Services
{
    public class CalculatorService : ICalculatorService
	{
		public CalculatorService(ICalculatorProcessor processor)
		{
			_processor = processor;
		}

		public void Run(string[] args)
		{
			if (args.Length > 0)
			{
				_processor.ProcessFile(args[0]);
			}
			else
			{
				_processor.ProcessConsole();
			}
		}

		private readonly ICalculatorProcessor _processor;
	}
}

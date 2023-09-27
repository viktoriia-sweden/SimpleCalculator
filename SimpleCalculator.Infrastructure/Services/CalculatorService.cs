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
				var file = args[0].Trim().ToLower();
				if (File.Exists(file))
				{
					_processor.ProcessFile(file);
				}
				else
				{
					Console.WriteLine($"[Warning].Provided file {file} was not found. Program is opened in \"Console\" mode.");
					_processor.ProcessConsole();
				}
			}
			else
			{
				_processor.ProcessConsole();
			}
		}

		private readonly ICalculatorProcessor _processor;
	}
}

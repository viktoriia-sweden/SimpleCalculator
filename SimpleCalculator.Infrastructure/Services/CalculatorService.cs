using Microsoft.Extensions.Logging;

using SimpleCalculator.Infrastructure.Processors;

namespace SimpleCalculator.Infrastructure.Services
{
	public class CalculatorService : ICalculatorService
	{
		public CalculatorService(ICalculatorProcessor processor, ILogger<CalculatorService> logger)
		{
			_processor = processor;
			_logger = logger;
		}

		/// <summary>
		/// Runs process based on file existence.
		/// </summary>
		/// <param name="args">Console arguments.</param>
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
					_logger.LogWarning($"Provided file {file} was not found. Program is opened in \"Console\" mode.");
					_processor.ProcessConsole();
				}
			}
			else
			{
				_processor.ProcessConsole();
			}
		}

		private readonly ICalculatorProcessor _processor;
		private readonly ILogger<CalculatorService> _logger;
	}
}

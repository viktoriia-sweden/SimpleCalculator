namespace SimpleCalculator.Infrastructure.Processors
{
	public interface ICalculatorProcessor
	{
		public void ProcessConsole();

		public void ProcessFile(string fileName);
	}
}

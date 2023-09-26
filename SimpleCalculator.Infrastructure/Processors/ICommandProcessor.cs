namespace SimpleCalculator.Infrastructure.Processors
{
    public interface ICommandProcessor
    {
		public void ProcessConsole();

		public void ProcessFile(string fileName);
	}
}

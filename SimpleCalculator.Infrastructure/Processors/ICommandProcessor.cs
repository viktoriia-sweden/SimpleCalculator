namespace SimpleCalculator.Infrastructure.Processors
{
	public interface ICommandProcessor
	{
		public void Process(string[] command);
	}
}

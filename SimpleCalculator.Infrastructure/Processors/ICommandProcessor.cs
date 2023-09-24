namespace SimpleCalculator.Infrastructure.Handlers
{
	public interface ICommandProcessor
	{
		public void Process(string[] args);
	}
}

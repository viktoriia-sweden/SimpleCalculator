namespace SimpleCalculator.Infrastructure.Services
{
	public interface ICommandResolver
	{
		public void Process(string[] args);

		public bool IsQuit();
	}
}

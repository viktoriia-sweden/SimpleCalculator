namespace SimpleCalculator.Infrastructure.Services
{
	public interface ICommandResolver
	{
		public bool IsQuit { get; }

		public void Process(string[] args);
	}
}

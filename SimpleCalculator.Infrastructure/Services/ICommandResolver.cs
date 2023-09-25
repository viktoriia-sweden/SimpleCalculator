namespace SimpleCalculator.Infrastructure.Services
{
	public interface ICommandResolver
	{
		public bool IsQuit { get; set; }

		public void Process(string[] args);
	}
}

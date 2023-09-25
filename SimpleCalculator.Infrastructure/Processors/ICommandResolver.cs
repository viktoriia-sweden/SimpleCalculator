namespace SimpleCalculator.Infrastructure.Processors
{
	public interface ICommandResolver
	{
		public bool IsQuit { get; set; }

		public void Process(string[] args);
	}
}

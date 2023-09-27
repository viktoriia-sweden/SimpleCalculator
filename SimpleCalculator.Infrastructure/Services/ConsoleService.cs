namespace SimpleCalculator.Infrastructure.Services
{
	public class ConsoleService : IConsoleService
	{
		public string? Read()
		{
			return Console.ReadLine();
		}

		public void Write(long value)
		{
			Console.WriteLine(value);
		}
	}
}

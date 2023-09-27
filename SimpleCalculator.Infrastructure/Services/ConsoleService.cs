namespace SimpleCalculator.Infrastructure.Services
{
	public class ConsoleService : IConsoleService
	{
		public ConsoleService() {
			Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
			{
				e.Cancel = true;
			};
		}

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

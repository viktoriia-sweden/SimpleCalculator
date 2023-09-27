namespace SimpleCalculator.Infrastructure.Services
{
	public interface IConsoleService
	{
		string? Read();

		void Write(long value);
	}
}

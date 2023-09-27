using SimpleCalculator.Infrastructure.Processors;

namespace SimpleCalculator.Infrastructure.Services
{
	public interface ICommandResolver
	{
		public bool IsQuit { get; }

		public ICommandProcessor Process(string[] args);
	}
}

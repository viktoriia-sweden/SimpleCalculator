using SimpleCalculator.Domain.Enums;
using SimpleCalculator.Infrastructure.Processors;
using SimpleCalculator.Infrastructure.Repositories;

namespace SimpleCalculator.Infrastructure.Services
{
	public class CommandResolver : ICommandResolver
	{
		public CommandResolver(IRegisterRepository registerRepository)
		{
			_registerRepository = registerRepository;
		}

		public bool IsQuit { get; private set; }

		public ICommandProcessor Process(string[] args)
		{
			if (Enum.TryParse<CommandType>(args[0], true, out var commandType))
			{
				if (commandType == CommandType.Print)
				{
					return new PrintProcessor(_registerRepository);
				}
				else if (commandType == CommandType.Quit)
				{
					IsQuit = true;
					return new QuitProcessor();
				}
				else
				{
					throw new ArgumentException($"Invalid command {string.Join(",", args)}");
				}
			}
			else
			{
				if (args.Length == 3 && Enum.TryParse<Operation>(args[1], true, out var operation))
				{
					return new OperationProcessor(_registerRepository);
				}
				else
				{
					throw new ArgumentException($"Invalid command {string.Join(",", args)}");
				}
			}
		}

		private readonly IRegisterRepository _registerRepository;
	}
}

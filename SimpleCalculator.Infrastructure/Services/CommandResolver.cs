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

		public bool IsQuit { get; set; } = false;

		public void Process(string[] args)
		{
			if (Enum.TryParse<CommandType>(args[0], true, out var commandType))
			{
				if (commandType == CommandType.Quit)
				{
					IsQuit = true;
				}
				if (commandType == CommandType.Print)
				{
					var processor = new PrintProcessor(_registerRepository);
					processor.Process(args[1]);
				}
			}
			else
			{
				if (args.Length == 3 && Enum.TryParse<Operation>(args[1], true, out var operation))
				{
					var processor = new OperationProcessor(_registerRepository);
					processor.Process(args[0], operation, args[2]);
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

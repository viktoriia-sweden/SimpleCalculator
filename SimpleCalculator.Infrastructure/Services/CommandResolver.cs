using SimpleCalculator.Domain.Entities;
using SimpleCalculator.Domain.Enums;
using SimpleCalculator.Infrastructure.Processors;
using SimpleCalculator.Infrastructure.Repositories;

namespace SimpleCalculator.Infrastructure.Services
{
	public class CommandResolver : ICommandResolver
	{
		public CommandResolver(IRegisterRepository registerRepository, IConsoleService consoleService)
		{
			_registerRepository = registerRepository;
			_consoleService = consoleService;
		}

		public bool IsQuit { get; private set; }

		/// <summary>
		/// Identifies arguments and correct processor.
		/// </summary>
		/// <param name="args">Command arguments.</param>
		/// <returns>A new instance of processor.</returns>
		/// <exception cref="ArgumentException"></exception>
		public ICommandProcessor Process(string[] args)
		{
			if (Enum.TryParse<CommandType>(args[0], true, out var commandType))
			{
				if (commandType == CommandType.Print)
				{
					return new PrintProcessor(_registerRepository, _consoleService);
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
				if (CommandsRules.TryGetCommandRules(CommandType.Operation, out var rules)
					&& args.Length == rules!.ArgsNumber
					&& Enum.TryParse<Operation>(args[1], true, out var _))
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
		private readonly IConsoleService _consoleService;
	}
}

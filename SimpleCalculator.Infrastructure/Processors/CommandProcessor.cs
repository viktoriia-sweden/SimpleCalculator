using SimpleCalculator.Domain.Enums;
using SimpleCalculator.Domain.Names;
using SimpleCalculator.Infrastructure.Handlers;
using SimpleCalculator.Infrastructure.Repositories;
using SimpleCalculator.Infrastructure.Validators;

namespace SimpleCalculator.Infrastructure.Processors
{
	public class CommandProcessor : ICommandProcessor
	{
		public CommandProcessor(IRegisterRepository registerRepository, ICommandValidator commandValidator)
		{
			_registerRepository = registerRepository;
			_commandValidator = commandValidator;
		}

		public void Process(string[] args)
		{
			if (Enum.TryParse<CommandType>(args[0], true, out var commandType))
			{
				if (commandType == CommandType.Quit)
				{
					if (_commandValidator.Validate(commandType, args))
					{
						Console.ReadKey();
					}
				}
				else if (commandType == CommandType.Print)
				{
					if (_commandValidator.Validate(commandType, args))
					{
						var processor = new PrintProcessor(_registerRepository);
						processor.Process(args[1]);
					}
				}
				else
				{
					Console.WriteLine($"Invalid command {args[0]}. Please check ReadMe file for allowed commands.");
				}
			}
			else
			{
				if (Enum.TryParse<Operation>(args[1], true, out var operation))
				{
					if (_commandValidator.Validate(CommandType.Operation, args))
					{
						var processor = new OperationProcessor(_registerRepository);
						processor.Process(args[0], operation, args[2]);
					}
				}
				else
				{
					Console.WriteLine($"Invalid operation {args[1]}. Please check ReadMe file for allowed operation.");
				}
			}
		}

		private readonly IRegisterRepository _registerRepository;
		private readonly ICommandValidator _commandValidator;
	}
}

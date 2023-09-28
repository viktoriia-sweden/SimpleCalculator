using SimpleCalculator.Domain.Entities;
using SimpleCalculator.Domain.Enums;
using SimpleCalculator.Infrastructure.Repositories;

namespace SimpleCalculator.Infrastructure.Processors
{
	public class OperationProcessor : ICommandProcessor
	{
		public OperationProcessor(IRegisterRepository registerRepository)
		{
			_registerRepository = registerRepository;
		}
		
		/// <summary>
		/// Calls repository to save new command if it is valid.
		/// </summary>
		/// <param name="command">Operation command.</param>
		public void Process(string[] command)
		{
			if (CommandsRules.TryGetCommandRules(CommandType.Operation, out var rules)
				&& command.Length == rules!.ArgsNumber
				&& Enum.TryParse<Operation>(command[1], true, out var operation))
			{
				_registerRepository.AddCommand(command[0], new Command { Operation = operation, Operand = command[2] });
			}
		}

		private readonly IRegisterRepository _registerRepository;
	}
}

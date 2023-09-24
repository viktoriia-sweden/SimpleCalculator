using SimpleCalculator.Domain.Entities;
using SimpleCalculator.Domain.Names;
using SimpleCalculator.Infrastructure.Repositories;

namespace SimpleCalculator.Infrastructure.Processors
{
	public class OperationProcessor
	{
		public OperationProcessor(IRegisterRepository registerRepository)
		{
			_registerRepository = registerRepository;
		}

		public void Process(string register, Operation operation, string operand) => _registerRepository.AddCommand(register, new Command { Operation = operation, Operand = operand });

		private readonly IRegisterRepository _registerRepository;
	}
}

using SimpleCalculator.Domain.Entities;
using SimpleCalculator.Domain.Neumes;
using SimpleCalculator.Infrastructure.Repositories;

namespace SimpleCalculator.Infrastructure.Processors
{
	public class OperationProcessor
	{
		public OperationProcessor(IRegisterRepository registerRepository)
		{
			this.registerRepository = registerRepository;
		}

		public void Process(string register, Operation operation, string operand) => this.registerRepository.AddCommand(register, new Command { Operation = operation, Operand = operand });

		private readonly IRegisterRepository registerRepository;
	}
}

using SimpleCalculator.Domain.Names;
using SimpleCalculator.Infrastructure.Repositories;

namespace SimpleCalculator.Infrastructure.Processors
{
	public class PrintProcessor
	{
		public PrintProcessor(IRegisterRepository registerRepository)
		{
			_registerRepository = registerRepository;
		}

		public void Process(string register)
		{
			var value = Calculate(register);
			Console.WriteLine(value);
		}

		private int Calculate(string register)
		{
			while (_registerRepository.GetCommandsCount(register) > 0)
			{
				var command = _registerRepository.GetCommand(register);
				var registerValue = _registerRepository.Get(register);

				if (int.TryParse(command.Operand, out var operandValue))
				{
					_registerRepository.Save(register, Calculate(registerValue, command.Operation, operandValue));
				}
				else
				{
					_registerRepository.Save(register, Calculate(registerValue, command.Operation, Calculate(command.Operand)));
				}
			}

			return _registerRepository.Get(register);
		}

		private static int Calculate(int registerValue, Operation operation, int value) => operation switch
		{
			Operation.Add => registerValue + value,
			Operation.Substract => registerValue - value,
			Operation.Multiply => registerValue * value,
			_ => throw new ArgumentOutOfRangeException(nameof(operation), $"Invalid operation: {operation}"),
		};

		private readonly IRegisterRepository _registerRepository;
	}
}

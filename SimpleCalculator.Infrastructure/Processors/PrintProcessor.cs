using SimpleCalculator.Domain.Enums;
using SimpleCalculator.Infrastructure.Repositories;
using SimpleCalculator.Infrastructure.Services;

namespace SimpleCalculator.Infrastructure.Processors
{
	public class PrintProcessor : ICommandProcessor
	{
		public PrintProcessor(IRegisterRepository registerRepository, IConsoleService consoleService)
		{
			_registerRepository = registerRepository;
			_consoleService = consoleService;
		}

		public void Process(string[] command)
		{
			var value = Calculate(command[1]);
			_consoleService.Write(value);
		}

		private long Calculate(string register)
		{
			if (checkedRegisters.Contains(register))
			{
				throw new InvalidOperationException($"[Error]. Program cannot evaluate {register} due to cyclic dependancy");
			}

			while (_registerRepository.GetCommandsCount(register) > 0)
			{
				var command = _registerRepository.GetCommand(register);
				var registerValue = _registerRepository.Get(register);

				if (long.TryParse(command.Operand, out var operandValue))
				{
					_registerRepository.Save(register, ApplyOperation(registerValue, command.Operation, operandValue));
				}
				else
				{
					checkedRegisters.Add(register);
					_registerRepository.Save(register, ApplyOperation(registerValue, command.Operation, Calculate(command.Operand)));
				}
			}

			checkedRegisters.Remove(register);
			return _registerRepository.Get(register);
		}

		private static long ApplyOperation(long registerValue, Operation operation, long value) => operation switch
		{
			Operation.Add => registerValue + value,
			Operation.Subtract => registerValue - value,
			Operation.Multiply => registerValue * value,
			_ => throw new ArgumentOutOfRangeException(nameof(operation), $"Invalid operation: {operation}"),
		};

		private readonly HashSet<string> checkedRegisters = new ();

		private readonly IRegisterRepository _registerRepository;
		private readonly IConsoleService _consoleService;
	}
}
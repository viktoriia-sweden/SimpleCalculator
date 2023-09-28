using SimpleCalculator.Domain.Entities;
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
			var value = CalculateWithCircularDependencies(command[1]);
			_consoleService.Write(value);
		}

		private long CalculateWithoutCircularDependencies(string register)
		{
			if (evaluatingRegisters.Contains(register))
			{
				throw new InvalidOperationException($"Program cannot evaluate register '{register}' due to circular dependency.");
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
					evaluatingRegisters.Add(register);
					_registerRepository.Save(register, ApplyOperation(registerValue, command.Operation, CalculateWithoutCircularDependencies(command.Operand)));
				}
			}

			evaluatingRegisters.Remove(register);
			return _registerRepository.Get(register);
		}

		private long CalculateWithCircularDependencies(string register)
		{
			while (_registerRepository.GetCommandsCount(register) > 0)
			{
				var command = _registerRepository.GetCommand(register);
				var operandValue = GetOperandValue(command);
				var registerValue = _registerRepository.Get(register);
				_registerRepository.Save(register, ApplyOperation(registerValue, command.Operation, operandValue));
			}

			return _registerRepository.Get(register);
		}

		private long GetOperandValue(Command command)
		{
			return long.TryParse(command.Operand, out var operandValue) ? operandValue : CalculateWithCircularDependencies(command.Operand);
		}

		private static long ApplyOperation(long registerValue, Operation operation, long value) => operation switch
		{
			Operation.Add => registerValue + value,
			Operation.Subtract => registerValue - value,
			Operation.Multiply => registerValue * value,
			_ => throw new ArgumentOutOfRangeException(nameof(operation), $"Invalid operation: {operation}"),
		};

		private readonly HashSet<string> evaluatingRegisters = new ();

		private readonly IRegisterRepository _registerRepository;
		private readonly IConsoleService _consoleService;
	}
}
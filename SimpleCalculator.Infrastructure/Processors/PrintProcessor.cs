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

		/// <summary>
		/// Runs register calculations and calls console service to write result to console.
		/// </summary>
		/// <param name="command">Print command.</param>
		public void Process(string[] command)
		{
			var value = CalculateWithCircularDependencies(command[1]);
			_consoleService.Write(value);
		}

		/// <summary>
		/// Calculates register value.
		/// </summary>
		/// <param name="register">Register.</param>
		/// <returns>Register value.</returns>
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

		/// <summary>
		/// Returns new regsiter value or calls CalculateWithCircularDependencies if value is register.
		/// </summary>
		/// <param name="command">Print command.</param>
		/// <returns>Operand value.</returns>
		private long GetOperandValue(Command command)
		{
			return int.TryParse(command.Operand, out var operandValue) ? operandValue : CalculateWithCircularDependencies(command.Operand);
		}

		private static long ApplyOperation(long registerValue, Operation operation, long value) => operation switch
		{
			Operation.Add => registerValue + value,
			Operation.Subtract => registerValue - value,
			Operation.Multiply => registerValue * value,
			_ => throw new ArgumentOutOfRangeException(nameof(operation), $"Invalid operation: {operation}"),
		};

		private readonly IRegisterRepository _registerRepository;
		private readonly IConsoleService _consoleService;
	}
}
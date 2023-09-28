using Microsoft.Extensions.Logging;

using SimpleCalculator.Domain.Entities;
using SimpleCalculator.Domain.Enums;

namespace SimpleCalculator.Infrastructure.Validators
{
	public class CommandValidator : ICommandValidator
	{
		public CommandValidator(ILogger<CommandValidator> logger) {
			_logger = logger;
		}

		/// <summary>
		/// Validates arguments.
		/// </summary>
		/// <param name="args">Command arguments.</param>
		/// <returns>Is command valid.</returns>
		public bool IsValid(string[]? args)
		{
			if (args == null)
			{
				_logger.LogError("Please enter not null command.");
				return false;
			}

			if (IsAllowedCommand(args, out var commandType)) return CheckCommand(args!, commandType);

			if (IsAllowedOperation(args))
			{
				if (!IsAlphaNumeric(args[0]))
				{
					_logger.LogError($"Argument {args[0]} is not alphanumeric.");
					return false;
				}

				if (!(IsAlphaNumeric(args[2]) || int.TryParse(args[2], out var _)))
				{
					_logger.LogError($"Argument {args[2]} should be alphanumeric register or integer value.");
					return false;
				}

				return true;
			}

			_logger.LogError($"Invalid command {string.Join(",", args)}. Please check README.md file for allowed commands.");
			return false;
		}

		private static bool IsAllowedOperation(string[] args) => CommandsRules.TryGetCommandRules(CommandType.Operation, out var rules) && args.Length == rules!.ArgsNumber && Enum.TryParse<Operation>(args[1], true, out var _);

		private static bool IsAllowedCommand(string[] args, out CommandType commandType) => Enum.TryParse(args[0], true, out commandType);

		private static bool IsAlphaNumeric(string str) => !string.IsNullOrEmpty(str) && str.ToCharArray().All(c => char.IsLetter(c) || char.IsNumber(c));

		/// <summary>
		/// Checkes non-operational commands. 
		/// </summary>
		/// <param name="args">Command arguments</param>
		/// <param name="commandType">Command type.</param>
		/// <returns>Is non-operational valid.</returns>
		private bool CheckCommand(string[] args, CommandType commandType)
		{
			if (CommandsRules.TryGetCommandRules(commandType, out var rules))
			{
				if (rules!.ArgsNumber != args.Length)
				{
					_logger.LogError($"Incorrect arguments number for command '{commandType}'.");
					return false;
				}

				foreach (var index in rules.AlphaNumericArgsRules)
				{
					if (!IsAlphaNumeric(args[index]))
					{
						_logger.LogError($"Argument {args[index]} is not alphanumeric.");
						return false;
					}
				}
			}

			return true;
		}

		private readonly ILogger<CommandValidator> _logger;
	}
}
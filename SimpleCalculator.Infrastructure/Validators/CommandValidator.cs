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

			_logger.LogError($"Invalid command {string.Join(",", args)}. Please check ReadMe file for allowed commands.");
			return false;
		}

		private static bool IsAllowedOperation(string[] args) => args.Length == operationCommandArgsCount && Enum.TryParse<Operation>(args[1], true, out var _);

		private static bool IsAllowedCommand(string[] args, out CommandType commandType) => Enum.TryParse(args[0], true, out commandType);

		private static bool IsAlphaNumeric(string str) => !string.IsNullOrEmpty(str) && str.ToCharArray().All(c => char.IsLetter(c) || char.IsNumber(c));

		private bool CheckCommand(string[] args, CommandType commandType)
		{
			if (commandRules.TryGetValue(commandType, out var rules))
			{
				if (rules.ArgsNumber != args.Length)
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

		private const int operationCommandArgsCount = 3;

		private readonly Dictionary<CommandType, CommandRules> commandRules = new ()
		{
			{CommandType.Quit, new CommandRules { ArgsNumber = 1 }},
			{CommandType.Print, new CommandRules { ArgsNumber = 2, AlphaNumericArgsRules = new List<int> { 1 } } },
		};

		private readonly ILogger<CommandValidator> _logger;
	}
}
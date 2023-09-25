using SimpleCalculator.Domain.Entities;
using SimpleCalculator.Domain.Enums;

namespace SimpleCalculator.Infrastructure.Validators
{
	public class CommandValidator : ICommandValidator
	{
		public bool IsValid(string[]? args)
		{
			if (args == null)
			{
				Console.WriteLine("Please enter not null command.");
				return false;
			}

			if (IsAllowedCommand(args, out var commandType)) return CheckCommand(args!, commandType);

			if (IsAllowedOperation(args))
			{
				if (!IsAlphaNumeric(args[0]))
				{
					Console.WriteLine($"Argument {args[0]} is not alphanumeric.");
					return false;
				}

				if (!(IsAlphaNumeric(args[2]) || long.TryParse(args[2], out var _)))
				{
					Console.WriteLine($"Argument {args[2]} should be alphanumeric register or integer value.");
					return false;
				}
			}

			Console.WriteLine($"Invalid command {string.Join(",", args)}. Please check ReadMe file for allowed commands.");
			return false;
		}

		private static bool IsAllowedOperation(string[] args) => args.Length == 3 && Enum.TryParse<Operation>(args[1], true, out var _);

		private static bool IsAllowedCommand(string[] args, out CommandType commandType) => Enum.TryParse(args[0], true, out commandType);

		public static bool IsAlphaNumeric(string str) => !string.IsNullOrEmpty(str) && str.ToCharArray().All(c => char.IsLetter(c) || char.IsNumber(c));

		private bool CheckCommand(string[] args, CommandType commandType)
		{
			if (commandRules.TryGetValue(commandType, out var rules))
			{
				if (rules.ArgsNumber != args.Length)
				{
					Console.WriteLine($"Incorrect arguments number for command '{commandType}'.");
					return false;
				}

				foreach (var index in rules.AlphaNumericArgsRules)
				{
					if (!IsAlphaNumeric(args[index]))
					{
						Console.WriteLine($"Argument {args[index]} is not alphanumeric.");
						return false;
					}
				}
			}

			return true;
		}

		// It also can be a config, but for simplisity I put it here 
		private readonly Dictionary<CommandType, CommandRules> commandRules = new ()
		{
			{CommandType.Quit, new CommandRules { ArgsNumber = 1 }},
			{CommandType.Print, new CommandRules { ArgsNumber = 2, AlphaNumericArgsRules = new List<int> { 1 } } },
		};
	}
}

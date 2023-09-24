using SimpleCalculator.Domain.Entities;
using SimpleCalculator.Domain.Enums;

namespace SimpleCalculator.Infrastructure.Validators
{
	public class CommandValidator : ICommandValidator
	{
		public bool Validate(CommandType commandType, string[] args)
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

		public static bool IsAlphaNumeric(string str) => !string.IsNullOrEmpty(str) && str.ToCharArray().All(c => char.IsLetter(c) || char.IsNumber(c));

		// It also can be a config, but for simplisity I put it here 
		private readonly Dictionary<CommandType, CommandRules> commandRules = new ()
		{
			{CommandType.Quit, new CommandRules { ArgsNumber = 1 }},
			{CommandType.Print, new CommandRules { ArgsNumber = 2, AlphaNumericArgsRules = new List<int> { 1 } } },
			{CommandType.Operation, new CommandRules { ArgsNumber = 3, AlphaNumericArgsRules = new List<int> { 0, 2 } } }
		};
	}
}

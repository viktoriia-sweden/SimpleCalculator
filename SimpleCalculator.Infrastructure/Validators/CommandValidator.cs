using SimpleCalculator.Domain.Entities;
using SimpleCalculator.Domain.Enums;
using SimpleCalculator.Infrastructure.Validators;

namespace SimpleCalculator
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
					var operandResult = IsAlphaNumeric(args[index]);
					if (!operandResult)
					{
						Console.WriteLine($"Argument {args[index]} is not alphanumeric for command '{commandType}'.");
						return false;
					}
				}
			}

			return true;
		}

		public static bool IsAlphaNumeric(string str)
		{
			if (string.IsNullOrEmpty(str)) return false;

			return str.ToCharArray().All(c => char.IsLetter(c) || char.IsNumber(c));
		}

		private readonly Dictionary<CommandType, ValidationRules> commandRules = new Dictionary<CommandType, ValidationRules>
		{
			{CommandType.Quit, new ValidationRules { ArgsNumber = 1 }},
			{CommandType.Print, new ValidationRules { ArgsNumber = 2, AlphaNumericArgsRules = new List<int> { 1 } } },
			{CommandType.Operation, new ValidationRules { ArgsNumber = 3, AlphaNumericArgsRules = new List<int> { 0, 2} } } };
	}
}

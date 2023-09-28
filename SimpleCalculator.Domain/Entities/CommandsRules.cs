using SimpleCalculator.Domain.Enums;

namespace SimpleCalculator.Domain.Entities
{
	public static class CommandsRules
	{
		public static bool TryGetCommandRules(CommandType type, out CommandRules? rules)
		{
			return commandRules.TryGetValue(type, out rules);
		}

		private static Dictionary<CommandType, CommandRules> commandRules = new()
		{
			{CommandType.Operation, new CommandRules { ArgsNumber = 3 }},
			{CommandType.Quit, new CommandRules { ArgsNumber = 1 }},
			{CommandType.Print, new CommandRules { ArgsNumber = 2, AlphaNumericArgsRules = new List<int> { 1 } } },
		};
	}
}

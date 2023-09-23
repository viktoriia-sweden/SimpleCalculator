namespace SimpleCalculator
{
	internal class Validator
	{
		public bool IsValid(string input)
		{
			var command = input.ToLower().Trim().Split(" ");

			if (command.Length == 1)
			{
				if (!IsCommandAllowed(command[0]))
				{
					Console.WriteLine($"[Error]. Invalid command '{command[0]}'. Please use allowed commands {string.Join(", ", Settings.ValidCommands)}.");
					return false;
				}

				if (IsCommandAllowed(command[0], 1))
				{
					Console.WriteLine($"[Error]. The command {command[0]} should not have arguments.");
					return false;
				}

				return true;
			}
			else if (command.Length == 2)
			{
				if (!IsCommandAllowed(command[0], 1))
				{
					Console.WriteLine($"[Error]. Invalid command '{command[0]}'. Please use allowed commands: {string.Join(", ", Settings.ValidCommandsWithArgument)}.");
					return false;
				}

				if (IsCommandAllowed(command[0]))
				{
					Console.WriteLine($"[Error]. The command {command[0]} requires register operand.");
					return false;
				}

				if (!IsAlphaNumeric(command[1]))
				{
					Console.WriteLine($"[Error]. Invalid register '{command[1]}'. Please use alphanumeric symbols for registers.");
					return false;
				}

				return true;
			}
			else if (command.Length == 3)
			{
				if (!IsAlphaNumeric(command[0]))
				{
					Console.WriteLine($"[Error]. Invalid register '{command[0]}'. Please use alphanumeric symbols for registers.");
					return false;
				}

				if (!IsAlphaNumeric(command[2]))
				{
					Console.WriteLine($"[Error]. Invalid register '{command[2]}'. Please use alphanumeric symbols for registers and numeric for values.");
					return false;
				}

				if (!Settings.ValidOperations.Contains(command[1], StringComparer.InvariantCultureIgnoreCase))
				{
					Console.WriteLine($"[Error]. Invalid calculator operation '{command[1]}'. Please use allowed operations: {string.Join(", ", Settings.ValidOperations)}.");
					return false;
				}

				return true;
			}
			else
			{
				Console.WriteLine("[Error].The command has an incorrect number of arguments. Please read ReadMe file for further details.");
				return false;
			}
		}

		private static bool IsCommandAllowed(string arg, int argsNumber = 0)
		{
			if (argsNumber == 0)
			{
				return Settings.ValidCommands.Contains(arg, StringComparer.InvariantCultureIgnoreCase);
			}
			else if (argsNumber == 1)
			{
				return Settings.ValidCommandsWithArgument.Contains(arg, StringComparer.InvariantCultureIgnoreCase);
			}

			return false;
		}

		public static bool IsAlphaNumeric(string str)
		{
			if (string.IsNullOrEmpty(str)) return false;

			return str.ToCharArray().All(c => char.IsLetter(c) || char.IsNumber(c));
		}
	}
}

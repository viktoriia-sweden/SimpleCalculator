namespace SimpleCalculator
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var registerValues = new Dictionary<string, int>();
			var registerCommandsDict = new Dictionary<string, Queue<Action>>();

			while (true)
			{
				var input = Console.ReadLine();
				var command = input.ToLower().Trim().Split(" ");
				var validator = new Validator();
				var isValid = validator.IsValid(input);

				if (command.Length == 3)
				{
					var commands = registerCommandsDict.GetValueOrDefault(command[0], new Queue<Action>());
					commands.Enqueue(new Action { Operation = Enum.Parse<Operation>(command[1], true), Operand = command[2] });
				}

				if (command.Length == 2)
				{
					var register = command[1];

					var result = Calculate2(register, registerCommandsDict, registerValues);
					Console.WriteLine(result);
				}

				if (command.Length == 1)
				{

				}
			}
		}

		private static int Calculate(int registerValue, Operation operation, int value) => operation switch
		{
			Operation.Add => registerValue + value,
			Operation.Substract => registerValue - value,
			Operation.Multiply => registerValue * value,
			_ => throw new ArgumentOutOfRangeException(nameof(operation), $"Invalid operation: {operation}"),
		};


		private static int Calculate2(string register, Dictionary<string, Queue<Action>> registerCommandsDict, Dictionary<string, int> dict)
		{
			if (int.TryParse(register, out var number))
			{
				return number;
			}

			var commands = registerCommandsDict.GetValueOrDefault(register, new Queue<Action>());
			while (commands.Count > 0)
			{
				var command = commands.Dequeue();
				var operation = command.Operation;
				var operand = command.Operand;
				var registerValue = dict.GetValueOrDefault(register, 0);

				if (int.TryParse(operand, out var operandValue))
				{
					dict[register] = Calculate(registerValue, operation, operandValue);
				}
				else
				{
					dict[register] = Calculate(registerValue, operation, Calculate2(operand, registerCommandsDict, dict));
				}
			}

			return dict[register];
		}
	}
}
using SimpleCalculator.Domain.Entities;

namespace SimpleCalculator.Infrastructure.Repositories
{
	/// <summary>
	/// Repository for managing register values and commands.
	/// </summary>
	public class RegisterRepository : IRegisterRepository
	{
		/// <summary>
		/// Saves register value.
		/// </summary>
		/// <param name="register">Register.</param>
		/// <param name="value">New register value.</param>
		public void Save(string register, long value)
		{
			if (!registersInfo.ContainsKey(register))
			{
				registersInfo[register] = new RegisterInfo();
			}

			registersInfo[register].CurrentValue = value;
		}

		/// <summary>
		/// Tries to get register value or return 0.
		/// </summary>
		/// <param name="register">Register.</param>
		/// <returns>Register value.</returns>
		public long Get(string register)
		{
			return registersInfo.ContainsKey(register) ? registersInfo[register].CurrentValue : 0;
		}

		/// <summary>
		/// Adds new register command. 
		/// </summary>
		/// <param name="register">Register.</param>
		/// <param name="command">New register command.</param>
		public void AddCommand(string register, Command command)
		{
			if (!registersInfo.ContainsKey(register))
			{
				registersInfo[register] = new RegisterInfo();
			}

			registersInfo[register].Commands.Enqueue(command);
		}

		/// <summary>
		/// Gets next register unevaluated command.
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns>Next register commands.</returns>
		public Command GetCommand(string register)
		{
			return registersInfo[register].Commands.Dequeue();
		}

		/// <summary>
		/// Checks register unevaluated commands count.
		/// </summary>
		/// <param name="register">Register.</param>
		/// <returns>Number of register commands.</returns>
		public int GetCommandsCount(string register)
		{
			return registersInfo.ContainsKey(register) ? registersInfo[register].Commands.Count : 0;
		}

		private readonly Dictionary<string, RegisterInfo> registersInfo = new();
	}
}

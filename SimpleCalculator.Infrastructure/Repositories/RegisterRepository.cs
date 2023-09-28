using SimpleCalculator.Domain.Entities;

namespace SimpleCalculator.Infrastructure.Repositories
{
	/// <summary>
	/// Repository for managing register values and its commands.
	/// </summary>
	public class RegisterRepository : IRegisterRepository
	{
		/// <summary>
		/// Saves value of register.
		/// </summary>
		/// <param name="register">Register.</param>
		/// <param name="value">Register new value.</param>
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
		/// Adds register new command. 
		/// </summary>
		/// <param name="register">Register.</param>
		/// <param name="command">Register new command.</param>
		public void AddCommand(string register, Command command)
		{
			if (!registersInfo.ContainsKey(register))
			{
				registersInfo[register] = new RegisterInfo();
			}

			registersInfo[register].Commands.Enqueue(command);
		}

		/// <summary>
		/// Gets register next command.
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns>Register next commands.</returns>
		public Command GetCommand(string register)
		{
			return registersInfo[register].Commands.Dequeue();
		}

		/// <summary>
		/// Checks register commands count.
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

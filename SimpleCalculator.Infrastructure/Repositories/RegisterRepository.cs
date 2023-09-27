using SimpleCalculator.Domain.Entities;

namespace SimpleCalculator.Infrastructure.Repositories
{
	public class RegisterRepository : IRegisterRepository
	{
		public void Save(string register, long value)
		{
			if (!registersInfo.ContainsKey(register))
			{
				registersInfo[register] = new RegisterInfo();
			}

			registersInfo[register].CurrentValue = value;
		}

		public long Get(string register)
		{
			return registersInfo.ContainsKey(register) ? registersInfo[register].CurrentValue : 0;
		}

		public void AddCommand(string register, Command command)
		{
			if (!registersInfo.ContainsKey(register))
			{
				registersInfo[register] = new RegisterInfo();
			}

			registersInfo[register].Commands.Enqueue(command);
		}

		public Command GetCommand(string register)
		{
			return registersInfo[register].Commands.Dequeue();
		}

		public int GetCommandsCount(string register)
		{
			return registersInfo.ContainsKey(register) ? registersInfo[register].Commands.Count : 0;
		}

		private readonly Dictionary<string, RegisterInfo> registersInfo = new();
	}
}

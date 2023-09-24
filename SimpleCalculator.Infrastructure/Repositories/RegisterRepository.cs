﻿using SimpleCalculator.Domain.Entities;

namespace SimpleCalculator.Infrastructure.Repositories
{
	public class RegisterRepository : IRegisterRepository
	{
		public void Save(string register, int value)
		{
			if (!registersInfo.ContainsKey(register))
			{
				registersInfo[register] = new RegisterInfo();
			}

			registersInfo[register].CurrentValue = value;
		}

		public int Get(string register)
		{
			return registersInfo[register].CurrentValue;
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
			return registersInfo[register].Commands.Count;
		}


		private readonly Dictionary<string, RegisterInfo> registersInfo = new();
	}
}

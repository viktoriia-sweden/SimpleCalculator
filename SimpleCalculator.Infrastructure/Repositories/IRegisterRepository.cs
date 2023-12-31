﻿using SimpleCalculator.Domain.Entities;

namespace SimpleCalculator.Infrastructure.Repositories
{
	public interface IRegisterRepository
	{
		public void Save(string register, long value);

		public long Get(string register);

		public void AddCommand(string register, Command command);

		public Command GetCommand(string register);

		public int GetCommandsCount(string register);
	}
}

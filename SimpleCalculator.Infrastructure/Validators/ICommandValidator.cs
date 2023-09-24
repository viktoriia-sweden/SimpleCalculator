using SimpleCalculator.Domain.Enums;

namespace SimpleCalculator.Infrastructure.Validators
{
	public interface ICommandValidator
	{
		public bool Validate(CommandType commandType, string[] args);
	}
}

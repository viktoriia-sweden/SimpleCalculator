using SimpleCalculator.Domain.Neumes;

namespace SimpleCalculator.Domain.Entities
{
	public class Command
	{
		public Operation Operation { get; set; }

		public string Operand { get; set; } = null!;
	}
}

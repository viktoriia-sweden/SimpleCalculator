using SimpleCalculator.Domain.Names;

namespace SimpleCalculator.Domain.Entities
{
	public class Command
	{
		public Operation Operation { get; set; }

		public string Operand { get; set; } = null!;
	}
}

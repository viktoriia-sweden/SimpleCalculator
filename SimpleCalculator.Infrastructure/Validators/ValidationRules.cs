using SimpleCalculator.Domain.Enums;

namespace SimpleCalculator.Domain.Entities
{
	public class ValidationRules
	{
		public int ArgsNumber { get; set;} 

		public List<(int, OperandRules)> Rules { get; set;} = new List<(int, OperandRules)>();
	}
}
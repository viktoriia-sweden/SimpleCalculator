namespace SimpleCalculator.Domain.Entities
{
	public class ValidationRules
	{
		public int ArgsNumber { get; set;} 

		public List<int> AlphaNumericArgsRules { get; set;} = new List<int>();
	}
}
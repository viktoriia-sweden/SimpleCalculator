namespace SimpleCalculator.Domain.Entities
{
	public class CommandRules
	{
		public int ArgsNumber { get; set;} 

		public List<int> AlphaNumericArgsRules { get; set;} = new List<int>();
	}
}
namespace SimpleCalculator.Domain.Entities
{
	public class RegisterInfo
	{
		public int CurrentValue { get; set; }

		public Queue<Command> Commands { get; set; } = new Queue<Command>();
	}
}

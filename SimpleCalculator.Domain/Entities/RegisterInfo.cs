namespace SimpleCalculator.Domain.Entities
{
	public class RegisterInfo
	{
		public long CurrentValue { get; set; }

		public Queue<Command> Commands { get; set; } = new Queue<Command>();
	}
}

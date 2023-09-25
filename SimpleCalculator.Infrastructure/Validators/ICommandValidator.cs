namespace SimpleCalculator.Infrastructure.Validators
{
	public interface ICommandValidator
	{
		public bool IsValid(string[]? args);
	}
}

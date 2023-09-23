namespace SimpleCalculator
{
	internal class Action
	{
		internal Operation Operation { get; set; }

		internal string Operand { get; set; } = null!;
	}

	enum Operation
	{
		Add,
		Substract,
		Multiply
	}
}

using AutoFixture;

using SimpleCalculator.Infrastructure.Processors;
using SimpleCalculator.Infrastructure.Repositories;
using FluentAssertions;

namespace SimpleCalculator.Infrastracture.Tests.Processors
{
	[TestClass]
	public class PrintProcessorTests
	{
		[TestInitialize]
		public void TestInitialize()
		{
			_fixture = new Fixture();
			_repository = new RegisterRepository();
			_processor = new PrintProcessor(_repository);
		}

		[TestMethod]
		public void TestOperationsOrder()
		{
			// arrange
			var register = _fixture.Create<string>();
			var addOperand = _fixture.Create<long>();
			var multiplyOperand = _fixture.Create<long>();
			var subtractOperand = _fixture.Create<long>();
			var expected = (addOperand * multiplyOperand - subtractOperand) * multiplyOperand;
			_repository.AddCommand(register, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = addOperand.ToString() });
			_repository.AddCommand(register, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Multiply, Operand = multiplyOperand.ToString() });
			_repository.AddCommand(register, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Subtract, Operand = subtractOperand.ToString() });
			_repository.AddCommand(register, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Multiply, Operand = multiplyOperand.ToString() });

			// act
			_processor.Process(new string[] { "print", register});

			// assert
			_repository.GetCommandsCount(register).Should().Be(0);
			_repository.Get(register).Should().Be(expected);
		}

		[TestMethod]
		public void TestLazyEvaluation()
		{
			// arrange
			var register = _fixture.Create<string>();
			var aditionalRegister = _fixture.Create<string>();
			var addOperand = _fixture.Create<long>();
			var multiplyOperand = _fixture.Create<long>();
			var expected = addOperand * multiplyOperand;
			_repository.AddCommand(aditionalRegister, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = addOperand.ToString() });
			_repository.AddCommand(register, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = aditionalRegister });
			_repository.AddCommand(register, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Multiply, Operand = multiplyOperand.ToString() });

			// act
			_processor.Process(new string[] { "print", register });

			// assert
			_repository.GetCommandsCount(register).Should().Be(0);
			_repository.Get(register).Should().Be(expected);
		}

		[TestMethod]
		public void TestLazyEvaluation_CopiedAddCommand()
		{
			// arrange
			var register = _fixture.Create<string>();
			var aditionalRegister = _fixture.Create<string>();
			var addOperand = _fixture.Create<long>();
			var expected = addOperand + addOperand;
			_repository.AddCommand(aditionalRegister, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = addOperand.ToString() });
			_repository.AddCommand(register, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = aditionalRegister });
			_repository.AddCommand(register, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = aditionalRegister });

			// act
			_processor.Process(new string[] { "print", register });

			// assert
			_repository.GetCommandsCount(register).Should().Be(0);
			_repository.Get(register).Should().Be(expected);
		}

		private Fixture _fixture;
		private RegisterRepository _repository;
		private PrintProcessor _processor;
	}
}
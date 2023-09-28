using AutoFixture;

using SimpleCalculator.Infrastructure.Processors;
using SimpleCalculator.Infrastructure.Repositories;
using FluentAssertions;
using SimpleCalculator.Infrastructure.Services;

namespace SimpleCalculator.Infrastracture.Tests.Processors
{
	// Example of acceptance tests 
	[TestClass]
	public class PrintProcessorTests
	{
		public PrintProcessorTests()
		{
			_fixture = new Fixture();
			_repository = new RegisterRepository();
			_consoleService = new ConsoleService();
			_processor = new PrintProcessor(_repository, _consoleService);
		}

		[TestMethod]
		public void TestOperationsOrder()
		{
			// arrange
			var register = _fixture.Create<string>();
			var addOperand = _fixture.Create<int>();
			var multiplyOperand = _fixture.Create<int>();
			var subtractOperand = _fixture.Create<int>();
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
			var addOperand = _fixture.Create<int>();
			var multiplyOperand = _fixture.Create<int>();
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
		public void Test_LargeVaules()
		{
			// arrange
			var register = _fixture.Create<string>();
			var addOperand = int.MaxValue;
			var expected = addOperand * (long)addOperand;
			_repository.AddCommand(register, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = addOperand.ToString() });
			_repository.AddCommand(register, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Multiply, Operand = addOperand.ToString() });

			// act
			_processor.Process(new string[] { "print", register });

			// assert
			_repository.GetCommandsCount(register).Should().Be(0);
			_repository.Get(register).Should().Be(expected);
		}

		[TestMethod]
		public void TestEmptyRegister()
		{
			// arrange
			var register = _fixture.Create<string>();

			// act
			_processor.Process(new string[] { "print", register });

			// assert
			_repository.GetCommandsCount(register).Should().Be(0);
			_repository.Get(register).Should().Be(0);
		}

		[TestMethod]
		public void TestLazyEvaluation_CopiedAddCommand()
		{
			// arrange
			var registerA = _fixture.Create<string>();
			var registerB = _fixture.Create<string>();
			var registerC = _fixture.Create<string>();
			var addOperand = _fixture.Create<int>();
			var expected = addOperand + addOperand;
			_repository.AddCommand(registerC, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = addOperand.ToString() });
			_repository.AddCommand(registerB, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = registerC });
			_repository.AddCommand(registerA, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = registerB });
			_repository.AddCommand(registerA, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = registerB });

			// act
			_processor.Process(new string[] { "print", registerA });

			// assert
			_repository.GetCommandsCount(registerA).Should().Be(0);
			_repository.Get(registerA).Should().Be(expected);
		}

		[TestMethod]
		public void TestLazyEvaluation_CircularDependency()
		{
			// arrange
			var registerA = _fixture.Create<string>();
			var registerB = _fixture.Create<string>();
			var registerC = _fixture.Create<string>();
			var addAOperand = _fixture.Create<int>();
			var addBOperand = _fixture.Create<int>();
			_repository.AddCommand(registerA, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = registerB });
			_repository.AddCommand(registerB, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = registerA });
			_repository.AddCommand(registerC, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = registerB });
			_repository.AddCommand(registerB, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = registerC });
			_repository.AddCommand(registerA, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = addAOperand.ToString() });
			_repository.AddCommand(registerB, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = addBOperand.ToString() });

			var expected = 4 * addBOperand + addAOperand;

			// act
			_processor.Process(new string[] { "print", registerB });

			// assert
			_repository.GetCommandsCount(registerA).Should().Be(0);
			_repository.Get(registerB).Should().Be(expected);
		}

		[TestMethod]
		public void TestLazyEvaluation_CircularDependency_SameRegister()
		{
			// arrange
			var registerA = _fixture.Create<string>();
			var addAOperand = _fixture.Create<int>();
			var subtractAOperand = _fixture.Create<int>();
			_repository.AddCommand(registerA, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = addAOperand.ToString() });
			_repository.AddCommand(registerA, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Multiply, Operand = registerA });
			_repository.AddCommand(registerA, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Subtract, Operand = subtractAOperand.ToString() });

			var expected = (addAOperand - subtractAOperand) * (addAOperand - subtractAOperand);

			// act
			_processor.Process(new string[] { "print", registerA });

			// assert
			_repository.GetCommandsCount(registerA).Should().Be(0);
			_repository.Get(registerA).Should().Be(expected);
		}

		[TestMethod]
		public void TestLazyEvaluation_CircularDependency_Multiply()
		{
			// arrange
			var registerA = _fixture.Create<string>();
			var registerB = _fixture.Create<string>();
			var addAOperand = _fixture.Create<int>();
			var addBOperand = _fixture.Create<int>();
			_repository.AddCommand(registerA, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = addAOperand.ToString() });
			_repository.AddCommand(registerB, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = addBOperand.ToString() });
			_repository.AddCommand(registerA, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Multiply, Operand = registerB });
			_repository.AddCommand(registerB, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Multiply, Operand = registerA });

			var expected = addAOperand * addAOperand * addBOperand;

			// act
			_processor.Process(new string[] { "print", registerA });

			// assert
			_repository.GetCommandsCount(registerA).Should().Be(0);
			_repository.Get(registerA).Should().Be(expected);
		}

		[TestMethod]
		public void TestLazyEvaluation_ManyDependencies()
		{
			// arrange
			var initialRegister = _fixture.Create<string>();
			var register = initialRegister;

			var i = 0;

			while (i < 100.000)
			{
				var additionalRegister = _fixture.Create<string>();
				_repository.AddCommand(register, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = additionalRegister });
				register = additionalRegister;
				i++;
			}

			var addOperand = _fixture.Create<int>();
			_repository.AddCommand(register, new Domain.Entities.Command { Operation = Domain.Enums.Operation.Add, Operand = addOperand.ToString() });

			var expected = addOperand;

			// act
			_processor.Process(new string[] { "print", initialRegister });

			// assert
			_repository.GetCommandsCount(initialRegister).Should().Be(0);
			_repository.Get(initialRegister).Should().Be(expected);
		}

		private readonly Fixture _fixture;
		private readonly RegisterRepository _repository;
		private readonly ConsoleService _consoleService;
		private readonly PrintProcessor _processor;
	}
}
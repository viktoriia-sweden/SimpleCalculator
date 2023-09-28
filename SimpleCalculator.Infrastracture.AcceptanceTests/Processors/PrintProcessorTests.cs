using AutoFixture;

using SimpleCalculator.Infrastructure.Processors;
using SimpleCalculator.Infrastructure.Repositories;
using FluentAssertions;
using SimpleCalculator.Infrastructure.Services;
using Microsoft.Win32;

namespace SimpleCalculator.Infrastracture.Tests.Processors
{
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
			var addOperand = _fixture.Create<long>();
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
			var addAOperand = _fixture.Create<long>();
			var addBOperand = _fixture.Create<long>();
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

		private readonly Fixture _fixture;
		private readonly RegisterRepository _repository;
		private readonly ConsoleService _consoleService;
		private readonly PrintProcessor _processor;
	}
}
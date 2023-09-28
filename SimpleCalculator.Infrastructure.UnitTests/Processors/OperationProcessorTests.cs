using AutoFixture;

using Moq;

using SimpleCalculator.Domain.Entities;
using SimpleCalculator.Domain.Enums;
using SimpleCalculator.Infrastructure.Processors;
using SimpleCalculator.Infrastructure.Repositories;

namespace SimpleCalculator.Infrastructure.UnitTests.Processors
{
	[TestClass]
	public class OperationProcessorTests
	{
		[TestInitialize]
		public void Initialize()
		{
			_fixture = new Fixture();
			_repository = new Mock<IRegisterRepository>(MockBehavior.Strict);
			_processor = new OperationProcessor(_repository.Object);
		}

		[TestMethod]
		public void ProcessTest()
		{
			// arrange
			var operation = _fixture.Create<Operation>();
			var register = _fixture.Create<string>();
			var operand = _fixture.Create<string>();

			string[] command = new string[] { register, operation.ToString(), operand };
			_repository
				.Setup(x => x.AddCommand(register, It.Is<Command>(y => 
					y.Operand.Equals(operand) && y.Operation.Equals(operation)
				)));

			// act
			_processor.Process(command);

			// assert
			_repository.Verify(x => x.AddCommand(register, It.IsAny<Command>()), Times.Once);
		}

		[TestMethod]
		public void ProcessTest_InvalidOperation()
		{
			// arrange
			var operation = _fixture.Create<string>();
			var register = _fixture.Create<string>();
			var operand = _fixture.Create<string>();

			string[] command = new string[] { register, operation, operand };

			// act
			_processor.Process(command);

			// assert
			_repository.Verify(x => x.AddCommand(register, It.IsAny<Command>()), Times.Never);
		}

		[TestMethod]
		public void ProcessTest_InvalidNumberOfArguments()
		{
			// arrange
			CommandsRules.TryGetCommandRules(CommandType.Operation, out var rules);

			string[] command = _fixture.CreateMany<string>(rules!.ArgsNumber + 1).ToArray();

			// act
			_processor.Process(command);

			// assert
			_repository.Verify(x => x.AddCommand(It.IsAny<string>(), It.IsAny<Command>()), Times.Never);
		}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		private Fixture _fixture;
		private Mock<IRegisterRepository> _repository;
		private OperationProcessor _processor;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	}
}

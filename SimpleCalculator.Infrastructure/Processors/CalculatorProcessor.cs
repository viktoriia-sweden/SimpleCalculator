using SimpleCalculator.Infrastructure.Services;
using SimpleCalculator.Infrastructure.Validators;

namespace SimpleCalculator.Infrastructure.Processors
{
    public class CalculatorProcessor : ICalculatorProcessor
    {
        public CalculatorProcessor
        (
            ICommandValidator commandValidator,
            ICommandResolver commandResolver
        )
        {
            _commandValidator = commandValidator;
            _commandResolver = commandResolver;
        }

        public void ProcessConsole()
        {
            while (!_commandResolver.IsQuit)
            {
                Process(Console.ReadLine());
            }
        }

        public void ProcessFile(string fileName)
        {
            using var sr = new StreamReader(fileName);

            string? line;
            while (!_commandResolver.IsQuit && (line = sr.ReadLine()) != null)
            {
                Process(line);
            }
        }

        private void Process(string? line)
        {
            var command = GetCommand(line);

            if (_commandValidator.IsValid(command))
            {
                var process = _commandResolver.Process(command!);
                process.Process(command!);
            }
        }

        private static string[]? GetCommand(string? str) => str?.Trim().Trim('\n').ToLower().Split(" ");

        private readonly ICommandValidator _commandValidator;
        private readonly ICommandResolver _commandResolver;
    }
}

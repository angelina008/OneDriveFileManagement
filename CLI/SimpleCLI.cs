using OneDriveFileManagement.CLI.Interfaces;
using OneDriveFileManagement.Services;
using OneDriveFileManagement.Utils;

namespace OneDriveFileManagement.CLI
{
    public class SimpleCLI
    {
        private readonly CommandFactory _commandFactory;

        public SimpleCLI(OneDriveService oneDriveService, FileComparator fileComparator)
        {
            _commandFactory = new CommandFactory(oneDriveService, fileComparator);
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                string[] parts = input.Split(' ');
                string commandName = parts[0];
                string[] args = parts.Skip(1).ToArray();

                if (commandName.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting CLI...");
                    break;
                }

                ICommand? command = _commandFactory.GetCommand(commandName);
                if (command != null)
                {
                    await command.ExecuteAsync(args);
                }
                else
                {
                    Console.WriteLine("Unknown command. Try 'upload', 'download', or 'compare'.");
                }
            }
        }
    }
}


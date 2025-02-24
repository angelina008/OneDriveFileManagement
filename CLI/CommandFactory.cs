using OneDriveFileManagement.CLI.Commands;
using OneDriveFileManagement.CLI.Interfaces;
using OneDriveFileManagement.Services;

namespace OneDriveFileManagement.CLI
{
    public class CommandFactory
    {
        private readonly Dictionary<string, ICommand> _commands;

        public CommandFactory(OneDriveService oneDriveService)
        {
            _commands = new Dictionary<string, ICommand>
            {
                { "upload", new UploadFileCommand(oneDriveService) },
                { "download", new DownloadFileCommand(oneDriveService) },
                { "compare", new CompareFilesCommand() },
                { "default", new RunDefaultCommand(oneDriveService) },
                { "help", new HelpCommand() }
            };
        }

        public ICommand? GetCommand(string commandName)
        {
            return _commands.TryGetValue(commandName, out var command) ? command : null;
        }
    }
}
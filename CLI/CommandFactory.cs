using OneDriveFileManagement.CLI.Commands;
using OneDriveFileManagement.CLI.Interfaces;
using OneDriveFileManagement.Services;
using OneDriveFileManagement.Utils;

namespace OneDriveFileManagement.CLI
{
    public class CommandFactory
    {
        private readonly Dictionary<string, ICommand> _commands;

        public CommandFactory(OneDriveService oneDriveService, FileComparator fileComparator)
        {
            _commands = new Dictionary<string, ICommand>
            {
                { "upload", new UploadFileCommand(oneDriveService) },
                { "download", new DownloadFileCommand(oneDriveService) },
                { "compare", new CompareFilesCommand(fileComparator) },
                { "default", new RunDefaultCommand(oneDriveService, fileComparator) },
                { "help", new HelpCommand() }
            };
        }

        public ICommand? GetCommand(string commandName)
        {
            return _commands.TryGetValue(commandName, out var command) ? command : null;
        }
    }
}
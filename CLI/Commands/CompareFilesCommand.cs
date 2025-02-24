using OneDriveFileManagement.CLI.Interfaces;
using OneDriveFileManagement.Utils;

namespace OneDriveFileManagement.CLI.Commands
{
    public class CompareFilesCommand : ICommand
    {
        private readonly FileComparator _fileComparator = new FileComparator();

        public CompareFilesCommand() { }

        public async Task ExecuteAsync(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: compare <uploadFileName.extension> <downloadFileName.extension>");
                return;
            }

            string originalFile = args[0]; //uploadFileName.extension
            string downloadedFile = args[1]; //downloadFileName.extension

            Console.WriteLine("Files are being compared...");
            await _fileComparator.CompareFilesAsync(originalFile, downloadedFile);
            
        }
    }
}

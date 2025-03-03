using OneDriveFileManagement.CLI.Interfaces;

namespace OneDriveFileManagement.CLI.Commands
{
    class HelpCommand : ICommand
    {
        public HelpCommand() { }

        public async Task ExecuteAsync(string[] args)
        {
            Console.WriteLine("OneDriveFileManagement.CLI commands: ");
            Console.WriteLine("upload -> Upload file on OneDrive -> Usage: compare <uploadFileName.extension> <downloadFileName.extension>");
            Console.WriteLine("download -> Download file from OneDrive to Local folder -> Usage: download <fileName.extension> <oneDriveFolder> <localDownloadFolder>");
            Console.WriteLine("compare -> Compares 2 files -> Usage: compare <uploadFileName.extension> <downloadFileName.extension>");
            Console.WriteLine("default -> Uploads file in specific oneDrive folder, downloads the same file in specific local folder, and compares the 2 files, all with predefined values.");
        }
    }
}

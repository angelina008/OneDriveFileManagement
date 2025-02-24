using OneDriveFileManagement.CLI.Interfaces;
using OneDriveFileManagement.Services;

namespace OneDriveFileManagement.CLI.Commands
{
    public class DownloadFileCommand : ICommand
    {
        private readonly OneDriveService _oneDriveService;

        public DownloadFileCommand(OneDriveService oneDriveService)
        {
            _oneDriveService = oneDriveService;
        }

        public async Task ExecuteAsync(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: download <fileName.extension> <oneDriveFolder> <localDownloadFolder>");
                return;
            }

            string fileName = args[0]; //fileName.extension
            string oneDriveFolder = args[1];
            string localDownloadFolder = args[2];

            await _oneDriveService.DownloadFileAsync(fileName, oneDriveFolder, localDownloadFolder);
            Console.WriteLine("File downloaded successfully.");
        }
    }
}

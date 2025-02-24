using OneDriveFileManagement.CLI.Interfaces;
using OneDriveFileManagement.Services;
using OneDriveFileManagement.Utils;

namespace OneDriveFileManagement.CLI.Commands
{
    class RunDefaultCommand : ICommand
    {
        private readonly FileComparator _fileComparator = new FileComparator();

        private readonly OneDriveService _oneDriveService;

        public RunDefaultCommand(OneDriveService oneDriveService)
        {
            _oneDriveService = oneDriveService;
        }

        public async Task ExecuteAsync(string[] args)
        {
            Console.WriteLine("Default values are being selected for uplaod/download of file...");
            await _oneDriveService.UploadFileAsync("NewFolder", "Text.txt");
            await _oneDriveService.DownloadFileAsync("Text.txt", "NewFolder", "DownloadFiles");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Text.txt");
            await _fileComparator.CompareFilesAsync(filePath, filePath);

        }
    }
}

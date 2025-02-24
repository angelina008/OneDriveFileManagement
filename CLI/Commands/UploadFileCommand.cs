using OneDriveFileManagement.CLI.Interfaces;
using OneDriveFileManagement.Services;

namespace OneDriveFileManagement.CLI.Commands
{
    public class UploadFileCommand : ICommand
    {
        private readonly OneDriveService _oneDriveService;

        public UploadFileCommand(OneDriveService oneDriveService)
        {
            _oneDriveService = oneDriveService;
        }

        public async Task ExecuteAsync(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: upload <destinationFolder> <fileName.extension>");
                return;
            }

            string destinationFolder = args[0];
            string file = args[1]; //fileName.extension


            await _oneDriveService.UploadFileAsync(destinationFolder, file);
            Console.WriteLine("File uploaded successfully.");
        }
    }
}

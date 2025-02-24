using Azure.Core;
using dotenv.net;
using Microsoft.Identity.Client;

namespace OneDriveFileManagement
{

    class Program 
    {
        static async Task Main(string[] args)
        {
            var authenticationService = new AuthenticationService();
            var oneDriveService = new OneDriveService(authenticationService);
            var fileComparator = new FileComparator();
            await oneDriveService.UploadFile("NewFolder", "Text.txt");
            await oneDriveService.DownloadFileAsync("Text.txt", "NewFolder", "DownloadFiles");
            await fileComparator.CompareFilesAsync("Text.txt", "Text.txt");
        }
    }
}
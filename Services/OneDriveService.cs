using Microsoft.Graph;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.Security;

namespace OneDriveFileManagement.Services
{
    public class OneDriveService
    {
        private readonly GraphServiceClient graphClient;
        private readonly AuthenticationService authenticationService;

        public OneDriveService(AuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;

            var authProvider = new BaseBearerTokenAuthenticationProvider(new AccessTokenProvider(authenticationService));
            graphClient = new GraphServiceClient(authProvider);
        }

        /// <summary>
        /// Upload a file to OneDrive
        /// </summary>
        /// <param name="folderName">Simple string</param>
        /// <param name="fileRelativePath">NameOfFile.fileExtension (e.g. text.txt) </param>
        /// <returns></returns>
        public async Task UploadFileAsync(string folderName, string fileRelativePath)
        {
            try
            {
                var drives = await graphClient.Me.Drives.GetAsync();
                var driveId = drives.Value.First().Id.ToString();

                var folders = await graphClient.Drives[driveId].Items["root"].Children.GetAsync();
                if (folders.Value.Any(x => x.Name == folderName))
                {

                    var folderId = folders.Value.First(x => x.Name == folderName).Id;
                    var filePath = fileRelativePath;
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var fileStream = new FileStream(fullPath, FileMode.Open))
                    {
                        var uploadedFile = graphClient.Drives[driveId].Items[folderId].ItemWithPath(Path.GetFileName(filePath)).Content;

                        await uploadedFile.PutAsync(fileStream);
                        Console.WriteLine($"File {filePath} uploaded successfully.");
                    }
                }
                else
                {
                    var driveItem = new DriveItem
                    {
                        Name = folderName,
                        Folder = new Folder { },
                    };

                    var addFolder = await graphClient.Drives[driveId].Items["root"].Children.PostAsync(driveItem);

                    var folderId = addFolder.Id;
                    var filePath = fileRelativePath;
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var fileStream = new FileStream(fullPath, FileMode.Open))
                    {
                        var uploadedFile = graphClient.Drives[driveId].Items[folderId].ItemWithPath(Path.GetFileName(filePath)).Content;

                        await uploadedFile.PutAsync(fileStream);
                        Console.WriteLine($"File {filePath} uploaded successfully.");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
            }
        }

        /// <summary>
        /// Download file from OneDrive to Local folder
        /// </summary>
        /// <param name="fileName">NameOfFile.fileExtension (e.g. text.txt)</param>
        /// <param name="folderName">Simple string, it's expected to be valid, existing Name</param>
        /// <param name="downloadPath">relative path to local folder (Name of the folder)</param>
        /// <returns></returns>
        public async Task DownloadFileAsync(string fileName, string folderName, string downloadPath)
        {
            try
            {

                downloadPath  = Path.Combine(Directory.GetCurrentDirectory(), downloadPath);
                var drives = await graphClient.Me.Drives.GetAsync();
                var driveId = drives.Value.First().Id.ToString();
                var folders = await graphClient.Drives[driveId].Items["root"].Children.GetAsync();
                var folderId = folders.Value.First(x => x.Name == folderName).Id;

                var files = await graphClient.Drives[driveId].Items[folderId].Children.GetAsync();
                var fileId = files.Value.First(x => x.Name == fileName).Id;

                using (var stream = await graphClient.Drives[driveId].Items[fileId].Content.GetAsync())
                {
                    using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }

                Console.WriteLine($"File downloaded to: {downloadPath}");
            
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
            }
        }

    }
}

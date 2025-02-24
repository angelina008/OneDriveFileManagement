using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions.Authentication;

namespace OneDriveFileManagement.Services
{
    public class OneDriveService
    {
        private readonly GraphServiceClient graphClient;

        public OneDriveService(AuthenticationService authenticationService)
        {
            var authProvider = new BaseBearerTokenAuthenticationProvider(new AccessTokenProvider(authenticationService));
            graphClient = new GraphServiceClient(authProvider);
        }

        /// <summary>
        /// Upload a file to OneDrive
        /// </summary>
        public async Task UploadFileAsync(string folderName, string fileRelativePath)
        {
            try
            {
                var driveId = await GetDriveIdAsync();
                var folderId = await GetFolderIdAsync(driveId, folderName);

                // If folder doesn't exist, create it
                if (folderId == null)
                {
                    folderId = await CreateFolderAsync(driveId, folderName);
                }

                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileRelativePath);
                await UploadFileToFolderAsync(driveId, folderId, fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
            }
        }

        /// <summary>
        /// Download file from OneDrive to Local folder
        /// </summary>
        public async Task DownloadFileAsync(string fileName, string folderName, string downloadPath)
        {
            try
            {
                var driveId = await GetDriveIdAsync();
                var folderId = await GetFolderIdAsync(driveId, folderName);

                if (folderId == null)
                {
                    Console.WriteLine($"Error: Folder {folderName} not found.");
                    return;
                }

                var files = await graphClient.Drives[driveId].Items[folderId].Children.GetAsync();
                var file = files.Value.FirstOrDefault(x => x.Name == fileName);

                if (file == null)
                {
                    Console.WriteLine($"Error: File {fileName} not found.");
                    return;
                }

                using (var stream = await graphClient.Drives[driveId].Items[file.Id].Content.GetAsync())
                using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(fileStream);
                }

                Console.WriteLine($"File downloaded to: {downloadPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
            }
        }

        #region Private Methods
        private async Task<string> GetDriveIdAsync()
        {
            try
            {
                var drives = await graphClient.Me.Drives.GetAsync();
                return drives.Value.First().Id.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting drive ID: {ex.Message}");
                return null;
            }
        }

        private async Task<string> GetFolderIdAsync(string driveId, string folderName)
        {
            try
            {
                var folders = await graphClient.Drives[driveId].Items["root"].Children.GetAsync();
                var folder = folders.Value.FirstOrDefault(x => x.Name == folderName);
                return folder?.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting folder ID for '{folderName}': {ex.Message}");
                return null;
            }
        }

        private async Task<string> CreateFolderAsync(string driveId, string folderName)
        {
            try
            {
                var driveItem = new DriveItem
                {
                    Name = folderName,
                    Folder = new Folder(),
                };

                var addFolder = await graphClient.Drives[driveId].Items["root"].Children.PostAsync(driveItem);
                return addFolder.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating folder '{folderName}': {ex.Message}");
                return null;
            }
        }

        private async Task UploadFileToFolderAsync(string driveId, string folderId, string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    var uploadedFile = graphClient.Drives[driveId].Items[folderId].ItemWithPath(Path.GetFileName(filePath)).Content;
                    await uploadedFile.PutAsync(fileStream);
                }
                Console.WriteLine($"File '{filePath}' uploaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file '{filePath}': {ex.Message}");
            }
        }

        #endregion
    }
}

using System.Security.Cryptography;

namespace OneDriveFileManagement.Utils
{ 
    public class FileComparator
    {
        public FileComparator() { }

        public async Task CompareFilesAsync(string originalFilePath, string downloadedFilePath)
        {
            string originalFileHash = await ComputeFileHashAsync(originalFilePath);

            string downloadedFileHash = await ComputeFileHashAsync(downloadedFilePath);

            if (originalFileHash.Equals(downloadedFileHash, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("The files are identical.");
            }
            else
            {
                Console.WriteLine("The files are different.");
            }
        }

        private static async Task<string> ComputeFileHashAsync(string filePath)
        {
            using (var sha256 = SHA256.Create())
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] hashBytes = await sha256.ComputeHashAsync(fileStream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant(); // Return as hex string
            }
        }
    }

}

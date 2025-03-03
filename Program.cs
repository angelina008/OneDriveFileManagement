using OneDriveFileManagement.CLI;
using OneDriveFileManagement.Services;
using OneDriveFileManagement.Utils;

namespace OneDriveFileManagement
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var oneDriveService = new OneDriveService(new AuthenticationService());
            var fileComparator = new FileComparator();
            var cli = new SimpleCLI(oneDriveService, fileComparator);
            await cli.RunAsync();
        }
    }
}
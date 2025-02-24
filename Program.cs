using OneDriveFileManagement.CLI;
using OneDriveFileManagement.Services;

namespace OneDriveFileManagement
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var oneDriveService = new OneDriveService(new AuthenticationService());
            var cli = new SimpleCLI(oneDriveService);
            await cli.RunAsync();
        }
    }
}
namespace OneDriveFileManagement.CLI.Interfaces
{
    public interface ICommand
    {
        Task ExecuteAsync(string[] args);
    }
}
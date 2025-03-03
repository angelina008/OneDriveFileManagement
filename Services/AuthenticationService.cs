using dotenv.net;
using Microsoft.Identity.Client;

public class AuthenticationService
{
    private readonly string clientId;
    private IPublicClientApplication publicClientApp;

    public AuthenticationService()
    {
        try
        {
            DotEnv.Load();
            clientId = Environment.GetEnvironmentVariable("CLIENT_ID");

            if (string.IsNullOrEmpty(clientId))
            {
                throw new Exception("CLIENT_ID is missing from environment variables.");
            }

            publicClientApp = PublicClientApplicationBuilder.Create(clientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, "common") // Use /common endpoint
                .WithRedirectUri("http://localhost") // Redirect URI for interactive login
                .Build();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing AuthenticationService: {ex.Message}");
            throw;
        }
    }

    public async Task<string> GetAccessTokenAsync()
    {
        string[] scopes = new[] { "User.Read", "Files.ReadWrite", "Files.Read", "Files.Read.All", "Files.ReadWrite.All" };

        AuthenticationResult result = null;

        try
        {
            var accounts = await publicClientApp.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();

            try
            {
                // Try to get the token silently
                result = await publicClientApp.AcquireTokenSilent(scopes, firstAccount)
                                              .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                Console.WriteLine("Silent authentication failed. Switching to interactive login.");

                // If silent acquisition fails, trigger an interactive login
                result = await publicClientApp.AcquireTokenInteractive(scopes)
                                              .WithPrompt(Prompt.SelectAccount) // Force account selection
                                              .ExecuteAsync();
            }
        }
        catch (MsalServiceException msalEx)
        {
            // MSAL-specific service errors (e.g., network issues, invalid configuration)
            Console.WriteLine($"MSAL Service Exception: {msalEx.Message}");
        }
        catch (MsalClientException msalClientEx)
        {
            // MSAL client-side issues (e.g., invalid request, authentication canceled)
            Console.WriteLine($"MSAL Client Exception: {msalClientEx.Message}");
        }
        catch (Exception ex)
        {
            // Catch any unexpected exceptions
            Console.WriteLine($"Unexpected error in GetAccessTokenAsync: {ex.Message}");
        }

        return result?.AccessToken ?? string.Empty;
    }
}

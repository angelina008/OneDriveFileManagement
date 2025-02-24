using dotenv.net;
using Microsoft.Identity.Client;

public class AuthenticationService
{
    private readonly string clientId;
    private readonly string clientSecret;
    private IPublicClientApplication publicClientApp;

    public AuthenticationService()
    {
        DotEnv.Load();
        clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
        clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");

        // Initialize PublicClientApplication
        publicClientApp = PublicClientApplicationBuilder.Create(clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, "common") // Use /common endpoint
            .WithRedirectUri("http://localhost") // Redirect URI for interactive login
            .Build();
    }

    // App-only authentication using /common (for personal accounts as well)
    public async Task<string> GetAccessTokenAsync()
    {
        string[] scopes = new[] { "User.Read", "Files.ReadWrite", "Files.Read", "Files.Read.All", "Files.ReadWrite.All" }; // Adjust scopes as needed

        var accounts = await publicClientApp.GetAccountsAsync();
        var firstAccount = accounts.FirstOrDefault();

        AuthenticationResult result;

        try
        {
            // Try to get the token silently
            result = await publicClientApp.AcquireTokenSilent(scopes, firstAccount)
                                          .ExecuteAsync();
        }
        catch (MsalUiRequiredException)
        {
            // If silent acquisition fails, trigger an interactive login
            result = await publicClientApp.AcquireTokenInteractive(scopes)
                                          .WithPrompt(Prompt.SelectAccount) // Force account selection
                                          .ExecuteAsync();
        }

        return result.AccessToken;
    }
}

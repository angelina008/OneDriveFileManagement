using Microsoft.Kiota.Abstractions.Authentication;

namespace OneDriveFileManagement
{
    public class AccessTokenProvider : IAccessTokenProvider
    {
        private readonly AuthenticationService authenticationService;

        public AccessTokenProvider(AuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public async Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object> additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
        {
            return await authenticationService.GetAccessTokenAsync();
        }

        public AllowedHostsValidator AllowedHostsValidator => new();
    }
}

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Frontend.Services.Firebase
{
    public class DevFirebaseConnection : IServicePrincipalProvider
    {
        private readonly IConfiguration _configuration;

        public DevFirebaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetKeyVaultSecretAsync()
        {
            try
            {
                string keyVaultUri = _configuration["ServicePrincipal:KEY_VAULT_URI"];
                string secretName = _configuration["ServicePrincipal:SECRET_NAME"];
                string tenantId = _configuration["ServicePrincipal:TENANT_ID"];
                string clientId = _configuration["ServicePrincipal:CLIENT_ID"];
                string clientSecret = _configuration["ServicePrincipal:CLIENT_SECRET"];

                var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

                var client = new SecretClient(new Uri(keyVaultUri), credential);

                var secret = await client.GetSecretAsync(secretName);
                return secret.Value.Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving service principal from Key Vault" + ex.Message);
            }

        }
    }
}

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Frontend.Models;

namespace Frontend.Services.Firebase
{
    public class DevFirebaseConnection : IServicePrincipalProvider
    {
        private readonly IConfiguration _configuration;

        public DevFirebaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<FirebaseSecret> GetKeyVaultSecretAsync()
        {
            try
            {
                string keyVaultUri = _configuration["ServicePrincipal:KEY_VAULT_URI"];
                string api_Key = _configuration["ServicePrincipal:API_KEY"];
                string project_id = _configuration["ServicePrincipal:PROJECT_ID"];
                string tenantId = _configuration["ServicePrincipal:TENANT_ID"];
                string clientId = _configuration["ServicePrincipal:CLIENT_ID"];
                string clientSecret = _configuration["ServicePrincipal:CLIENT_SECRET"];

                var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

                var client = new SecretClient(new Uri(keyVaultUri), credential);

                KeyVaultSecret firebaseApiSecret = await client.GetSecretAsync(api_Key);
                string firebaseApiKey = firebaseApiSecret.Value;

                KeyVaultSecret firebaseProjectIdSecret = await client.GetSecretAsync(project_id);
                string projectId = firebaseProjectIdSecret.Value;

                return new FirebaseSecret
                {
                    FirebaseApiKey = firebaseApiKey,
                    ProjectId = projectId
                };

            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving service principal from Key Vault" + ex.Message);
            }

        }
    }
}

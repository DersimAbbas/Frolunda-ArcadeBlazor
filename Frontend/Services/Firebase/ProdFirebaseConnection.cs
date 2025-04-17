using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Frontend.Models;

namespace Frontend.Services.Firebase
{
    public class ProdFirebaseConnection : IServicePrincipalProvider
    {
        private readonly IConfiguration _configuration;

        public ProdFirebaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<FirebaseSecret> GetKeyVaultSecretAsync()
        {
            string keyVaultUrl = _configuration["KeyVault:Uri"];
            string api_Key = _configuration["KeyVault:API_KEY"];
            string project_id = _configuration["KeyVault:PROJECT_ID"];
            if (string.IsNullOrEmpty(keyVaultUrl))
            {
                throw new Exception("Key Vault URI is not set in the configuration.");
            }
            var credential = new DefaultAzureCredential();
            var client = new SecretClient(new Uri(keyVaultUrl), credential);
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
    }
}

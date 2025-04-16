using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Frontend.Services.Firebase
{
    public class ProdFirebaseConnection : IServicePrincipalProvider
    {
        private readonly IConfiguration _configuration;

        public ProdFirebaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetKeyVaultSecretAsync()
        {
            string keyVaultUrl = _configuration["KeyVault:Uri"];

            if (string.IsNullOrEmpty(keyVaultUrl))
            {
                throw new Exception("Key Vault URI is not set in the configuration.");
            }

            var credential = new DefaultAzureCredential();
            var client = new SecretClient(new Uri(keyVaultUrl), credential);
            var secretName = _configuration["KeyVault:SecretName"];

            var secret = await client.GetSecretAsync(secretName);

            return secret.Value.Value;
        }
    }
}

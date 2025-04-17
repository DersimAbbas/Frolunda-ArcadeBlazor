using Frontend.Models;

namespace Frontend.Services.Firebase
{
    public interface IServicePrincipalProvider
    {
        Task<FirebaseSecret> GetKeyVaultSecretAsync();
    }
}

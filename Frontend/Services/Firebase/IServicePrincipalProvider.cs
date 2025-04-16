namespace Frontend.Services.Firebase
{
    public interface IServicePrincipalProvider
    {
        Task<string> GetKeyVaultSecretAsync();
    }
}

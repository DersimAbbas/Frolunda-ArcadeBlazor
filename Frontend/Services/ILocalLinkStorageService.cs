namespace Frontend.Services
{
    public interface ILocalLinkStorageService
    {
        Dictionary<string, string> ImageMap { get; }
        Task LoadImageMapAsync();
        Task SaveImageMapAsync(Dictionary<string, string> imageMap);
        string? GetImageLink(string productName);
    }
}

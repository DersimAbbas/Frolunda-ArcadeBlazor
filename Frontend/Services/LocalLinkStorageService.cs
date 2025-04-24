using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using System.Text.Json;


namespace Frontend.Services
{
    public class LocalLinkStorageService : ILocalLinkStorageService
    {
        private readonly ProtectedLocalStorage _storage;
        private const string ImageMapKey = "imageMap";
        private readonly IJSRuntime _js; 
        public Dictionary<string, string> ImageMap { get; private set; } = new();

        public LocalLinkStorageService(ProtectedLocalStorage storage, IJSRuntime js)
        {
            _storage = storage;
            _js = js;
        }

        public async Task SaveImageMapAsync(Dictionary<string, string> imageMap)
        {
            var json = JsonSerializer.Serialize(imageMap);
            await _js.InvokeVoidAsync("localStorage.setItem", ImageMapKey, json);
        }

        public async Task LoadImageMapAsync()
        {
            var json = await _js.InvokeAsync<string>("localStorage.getItem", ImageMapKey);
            if (string.IsNullOrWhiteSpace(json) || !IsValidJson(json))
            {
                ImageMap = new Dictionary<string, string>();
                return;
            }

            ImageMap = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }

        private bool IsValidJson(string json)
        {
            try
            {
                JsonDocument.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public string? GetImageLink(string productName)
        {
            if (ImageMap.TryGetValue(productName, out var url))
            {
                return url;
            }
            return null;
        }
    }
}

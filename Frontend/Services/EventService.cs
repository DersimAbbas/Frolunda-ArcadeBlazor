using Frontend.Services.Interfaces;
using static Frontend.Components.User.Components.EventCarousel;

namespace Frontend.Services
{
    public class EventService(HttpClient _httpClient) :IEventService
    {

        private IEventService _eventServiceImplementation;
        public async Task<Models.Event> GetEventByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<Models.Event>($"api/events/{id}");
        }

        public async Task<List<Models.Event>?> GetAllEventsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Models.Event>?>("api/events");
        }

        public async Task<bool> AddEventAsync(Models.Event _event)
        {
            var response = await _httpClient.PostAsJsonAsync("api/events", _event);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateEvent(string id, Models.Event _event)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/events/{id}", _event);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteEvent(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/events/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}

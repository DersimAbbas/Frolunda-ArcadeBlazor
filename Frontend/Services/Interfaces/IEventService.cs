using Frontend.Models;

namespace Frontend.Services.Interfaces
{
    public interface IEventService
    {
        Task<Models.Event> GetEventByIdAsync(string id);
        Task<List<Models.Event>?> GetAllEventsAsync();
        Task<bool> AddEventAsync(Models.Event _event);
        Task<bool> UpdateEvent(string id, Models.Event _event);
        Task<bool> DeleteEvent(string id);
    }
}

using EvalD2P2.Entities;

namespace EvalD2P2.Services.Contracts;

public interface IEventService
{
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<Event> GetEventByIdAsync(Guid id);
    Task AddEventAsync(Event @event);
    Task UpdateEventAsync(Event @event);
    Task DeleteEventAsync(Guid id);
}
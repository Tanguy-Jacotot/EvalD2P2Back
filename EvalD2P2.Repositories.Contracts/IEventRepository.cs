using EvalD2P2.Entities;

namespace EvalD2P2.Repositories.Contracts;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<Event> GetEventByIdAsync(Guid id);
    Task AddEventAsync(Event @event);
    Task UpdateEventAsync(Event @event);
    Task DeleteEventAsync(Guid id);
}
using EvalD2P2.Entities;
using EvalD2P2.Repositories.Contracts;
using EvalD2P2.Services.Contracts;

namespace EvalD2P2.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _repository;
    
    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }
    
    public async Task AddEventAsync(Event @event)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }
        await _repository.AddEventAsync(@event);
    }
    
    public async Task DeleteEventAsync(Guid id)
    {
        await _repository.DeleteEventAsync(id);
    }
    
    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        return await _repository.GetAllEventsAsync();
    }

    public async Task<Event> GetEventByIdAsync(Guid id)
    {
        return await _repository.GetEventByIdAsync(id);
    }
    
    public async Task UpdateEventAsync(Event @event)
    {
        await _repository.UpdateEventAsync(@event);
    }
}
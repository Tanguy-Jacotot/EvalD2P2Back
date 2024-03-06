using EvalD2P2.Entities;
using EvalD2P2.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EvalD2P2.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;
    
    public EventRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        return await _context.Events.ToListAsync();
    }
    
    public async Task<Event> GetEventByIdAsync(Guid id)
    {
        return await _context.Events.FirstOrDefaultAsync(e => e.IdEvent == id);
    }
    
    public async Task AddEventAsync(Event @event)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }
        _context.Events.Add(@event);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateEventAsync(Event @event)
    {
        var existingEvent = await _context.Events.FindAsync(@event.IdEvent);
        if (existingEvent == null)
        {
            throw new Exception("Impossible de mettre a jour avec des champs null");
        }
        _context.Entry(existingEvent).CurrentValues.SetValues(@event);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteEventAsync(Guid id)
    {
        var @event = await _context.Events.FindAsync(id);
        if (@event == null)
        {
            throw new Exception("Impossible de supprimer un élément null");
        }
        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();
    }
}
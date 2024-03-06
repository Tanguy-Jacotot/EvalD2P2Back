using System.ComponentModel.DataAnnotations;

namespace EvalD2P2.Entities;

public class Event
{
    [Key]
    public Guid IdEvent { get; set; } = Guid.NewGuid();
    
    [MaxLength(50)]
    public string? Title { get; set; }
    
    [MaxLength(50)]
    public string? Description { get; set; }
    
    public DateTime Date { get; set; }
    
    [MaxLength(50)]
    public string? Location { get; set; }
}
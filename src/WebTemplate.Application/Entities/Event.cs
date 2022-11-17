using WebTemplate.Application.Shared;

namespace WebTemplate.Application.Entities;

public class Event : BaseEntity
{
    public DateTimeOffset Date { get; set; }
    public string Entity { get; set; } = null!;
    public string EventType { get; set; } = null!;
    public string? Original { get; set; }
    public string? Current { get; set; } = null!;

}

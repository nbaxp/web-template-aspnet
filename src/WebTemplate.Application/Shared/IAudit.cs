namespace WebTemplate.Application.Shared;

public interface IAudit
{
    string? CreatedBy { get; set; }
    string? ModifiedBy { get; set; }
    DateTimeOffset? CreatedAt { get; set; }
    DateTimeOffset? ModifiedAt { get; set; }
}

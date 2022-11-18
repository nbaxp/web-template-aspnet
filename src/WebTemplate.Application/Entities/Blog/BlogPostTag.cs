using WebTemplate.Application.Shared;

namespace WebTemplate.Application.Entities.Blog;

public class BlogPostTag : BaseEntity, IConcurrencyStamp
{
    public string Name { get; set; } = null!;

    public int BlogPostCount { get; set; }
    public string? ConcurrencyStamp { get; set; }
}

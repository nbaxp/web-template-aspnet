using WebTemplate.Web.Models;

namespace WebTemplate.Web;

public class PaginationViewModel<T>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Query { get; set; }
    public string? OrderBy { get; set; }
    public int TotalCount { get; set; }
    public List<T> Items { get; set; } = new List<T>();
    public QeuryTemp? Test { get; set; }
    public DateTimeOffset? DateTimeOffset { get; set; }
}

public class QeuryTemp
{
    public string? Test { get; set; }
}


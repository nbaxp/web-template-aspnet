using System.ComponentModel.DataAnnotations;

namespace WebTemplate.Application.Shared;

public abstract class BaseEntity
{
    [DataType("Key")]
    public Guid Id { get; set; }

    public BaseEntity()
    {
        this.Id = Guid.NewGuid();
    }
}

using System.ComponentModel.DataAnnotations;

namespace EclipseWorks.Domain.Models;

public abstract class Entity
{
    [Key]
    public int Id { get; set; }
}

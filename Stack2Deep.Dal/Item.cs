using System.ComponentModel.DataAnnotations;

namespace Stack2Deep.Dal;

public abstract class Item
{
    [Key]
    public long Id { get; set; }
}
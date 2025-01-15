using System.ComponentModel.DataAnnotations;

namespace Phaedra.Server.Models.Entities.Documents;

public class TextSegment
{
    [Key]
    public int Id { get; set; }
    [MaxLength(255)]
    public string Text { get; set; } = string.Empty;
    [Required]
    public int ContentBlockId { get; set; }

    public int Order { get; set; } = 0;
    public TextStyle TextStyle { get; set; } = new();
}
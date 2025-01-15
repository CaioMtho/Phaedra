using System.ComponentModel.DataAnnotations;

namespace Phaedra.Server.Models.Entities.Documents;

public class Document
{
    [Key]
    public int Id { get; set; }
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters long.")]
    [MaxLength(50, ErrorMessage = "Document name must be between 3 and 50 characters")]
    public string Title { get; set; } = "New Document";
    public List<ContentBlock> ContentBlocks { get; set; } = [new ContentBlock()];
    [Required(ErrorMessage = "ProjectId is required")]
    public int ProjectId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
}
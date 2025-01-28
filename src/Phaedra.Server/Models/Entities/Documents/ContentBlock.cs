using System.ComponentModel.DataAnnotations;

namespace Phaedra.Server.Models.Entities.Documents;

public class ContentBlock : IDocumentComponent
{
    public int Id { get; set; }
    public int Order { get; set; } = 0;
    public List<TextSegment> TextSegments { get; set; } = [new TextSegment()];
    [Required]
    public int DocumentId { get; set; }
}
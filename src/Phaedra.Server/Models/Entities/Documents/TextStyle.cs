using System.ComponentModel.DataAnnotations;

namespace Phaedra.Server.Models.Entities.Documents;

public class TextStyle
{
    [Key]
    public int Id { get; set; }
    [MaxLength(50, ErrorMessage = "Font name cannot be longer than 50 characters.")]
    public string FontFamily { get; set; } = "noto sans";
    public float FontSize { get; set; } = 12;
    [MaxLength(50, ErrorMessage = "Font color cannot be longer than 50 characters.")]
    public string Color { get; set; } = "black";
    public bool IsBold { get; set; } = false;
    public bool IsItalic { get; set; } = false;
}
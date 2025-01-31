namespace Phaedra.Server.DTO.Project
{
    public class CreateProjectDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ICollection<int> AuthorIds { get; set; } = null!;
    }
}

namespace KL.Shared.Resources;

public class Resource
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Source { get; set; }
    public List<string> Comments { get; set; } = new();
}
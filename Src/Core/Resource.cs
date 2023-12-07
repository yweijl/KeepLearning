namespace Core;

public class Resource
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Source { get; set; }
    public required List<string> Comments { get; set; }
}
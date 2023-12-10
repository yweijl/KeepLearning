using System.ComponentModel.DataAnnotations;

namespace KL.Shared.Resources;

public class AddResourceRequest
{
    [Required(ErrorMessage = "Geef een naam op")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "Geef een Beschrijving")]
    public string Description { get; set; } = default!;

    [Required(ErrorMessage = "Voer een bron in")]
    public string Source { get; set; } = default!;

    public List<string> Comments { get; set; } = new();
}
namespace DulceFe.API.Social.Domain.Model.Aggregates;

public class Testimonial
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public int Stars { get; set; }
    public string Role { get; set; } = "Cliente Verificado";
    public bool IsPinned { get; set; } = false;
}

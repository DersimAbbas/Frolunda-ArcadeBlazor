namespace Frontend.Models;

public class CartProductDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string ImageLink { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is CartProductDto dto && Id == dto.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
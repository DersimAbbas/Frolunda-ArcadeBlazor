namespace Frontend.Models
{
    public class ProductFilter
    {
        public string? SearchTerm { get; set; } = string.Empty;
        public string? SelectedGenre { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
    }
}

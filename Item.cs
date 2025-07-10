public class Item
{
    public int Article { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
    public int? CellId { get; set; }
    public int ClientId { get; set; }

    public int BoxId { get; set; } // Связь с коробкой
    public bool IsAccepted { get; set; } = false;
    
}
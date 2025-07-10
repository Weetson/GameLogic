public class Box
{
    public int Id { get; set; }
    public List<Item> Items { get; set; } = new List<Item>();
    public bool IsFullyAccepted => Items.All(i => i.IsAccepted);   
}
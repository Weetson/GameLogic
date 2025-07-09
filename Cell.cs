public class Cell
{
    public int Id { get; set; }
    public List<Item> Items { get; set; } = new();

    public int Capacity { get; set; } = 5;  // или сколько хочешь
    public bool IsFull => Items.Count >= Capacity;
}

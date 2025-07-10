public class Delivery
{
    public int Id { get; set; }
    public List<Box> Boxes { get; set; } = new List<Box>();
    public DateTime CreationTime { get; set; }
    public TimeSpan Deadline { get; set; }
    public bool IsCompleted => Boxes.All(b => b.IsFullyAccepted);
}
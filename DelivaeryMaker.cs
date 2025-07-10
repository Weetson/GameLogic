public class DeliveryMaker
{
    private Random _random = new Random();

    // Генерация поставки из JSON
    public Delivery GenerateDeliveryFromJson(string jsonPath)
    {
        if (!System.IO.File.Exists(jsonPath))
            throw new FileNotFoundException($"Файл {jsonPath} не найден.");

        var jsonText = System.IO.File.ReadAllText(jsonPath);
        var rawData = System.Text.Json.JsonSerializer.Deserialize<List<RawProduct>>(jsonText);

        if (rawData == null || rawData.Count == 0)
            throw new Exception("JSON не содержит данных.");

        var delivery = new Delivery
        {
            Id = _random.Next(1, 10000),
            CreationTime = DateTime.Now,
            Deadline = TimeSpan.FromSeconds(_random.Next(300, 600))
        };

        int count = _random.Next(70, 111); // Случайное количество товаров
        var selected = rawData.OrderBy(x => _random.Next()).Take(count).ToList();

        int boxId = 1;
        var boxItems = new List<Item>();
        foreach (var product in selected)
        {
            boxItems.Add(new Item
            {
                Article = int.TryParse(product.article, out var id) ? id : 0,
                Name = product.name,
                Price = decimal.TryParse(product.price.Replace(",", "."), out var pr) ? pr : 0,
                Image = product.image,
                ClientId = _random.Next(1, 1000),
                BoxId = boxId
            });

            if (boxItems.Count >= _random.Next(20, 30))
            {
                delivery.Boxes.Add(new Box { Id = boxId++, Items = boxItems });
                boxItems = new List<Item>();
            }
        }
        if (boxItems.Count > 0)
            delivery.Boxes.Add(new Box { Id = boxId, Items = boxItems });

        Console.WriteLine($"🚚 Сгенерирована поставка #{delivery.Id} из JSON: {delivery.Boxes.Count} коробок, " +
            $"{delivery.Boxes.Sum(b => b.Items.Count)} товаров, дедлайн: {delivery.Deadline.TotalSeconds} сек.");
        return delivery;
    }
}

public class RawProduct
{
    public string article { get; set; }
    public string name { get; set; }
    public string price { get; set; }
    public string image { get; set; }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

public class Delivery
{
    private readonly Random _random = new();

    public List<Item> GenerateDelivery(string jsonPath, List<Cell> availableCells)
    {
        if (!File.Exists(jsonPath))
            throw new FileNotFoundException($"Файл {jsonPath} не найден.");

        var jsonText = File.ReadAllText(jsonPath);
        var rawData = JsonSerializer.Deserialize<List<RawProduct>>(jsonText);

        if (rawData == null || rawData.Count == 0)
            throw new Exception("JSON не содержит данных.");

        // Случайное количество товаров
        int count = _random.Next(70, 111);
        var selected = rawData.OrderBy(x => _random.Next()).Take(count).ToList();

        // Конвертируем в Item
        var items = selected.Select(p => new Item
        {
            Article = int.TryParse(p.article, out var id) ? id : 0,
            Name = p.name,
            Price = decimal.TryParse(p.price.Replace(",", "."), out var pr) ? pr : 0,
            Image = p.image
        }).ToList();

        // Разбиваем на группы по 1–5 товаров и распределяем по ячейкам
        int itemIndex = 0;
        foreach (var cell in availableCells)
        {
            if (itemIndex >= items.Count)
                break;

            int groupSize = _random.Next(1, 6); // от 1 до 5
            var group = items.Skip(itemIndex).Take(groupSize).ToList();

            foreach (var item in group)
            {
                item.CellId = cell.Id;
                cell.Items.Add(item);
            }

            itemIndex += groupSize;
        }

        return items;
    }

    private class RawProduct
    {
        public string article { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string image { get; set; }
    }
}

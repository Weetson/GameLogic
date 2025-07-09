class Program {

    public static void Main()
    {
        var cells = Enumerable.Range(1, 30).Select(id => new Cell { Id = id }).ToList();
        var delivery = new Delivery();
        var items = delivery.GenerateDelivery("wb_products.json", cells);

        Console.WriteLine($"Доставлено {items.Count} товаров.");
        foreach (var cell in cells.Where(c => c.Items.Any()))
        {
            Console.WriteLine($"Ячейка {cell.Id}: {cell.Items.Count} товаров");
        }

        // var item_one = cells[0].Items[0];
        // Console.WriteLine(item_one.CellId);
    }
    
}
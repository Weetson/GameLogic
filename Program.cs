class Program {

    public static void Main()
    {
        var deliveryGen = new Delivery();
        var warehouse = new Warehouse();

        var items = deliveryGen.GenerateDelivery("wb_products.json", warehouse.Cells);
        warehouse.AcceptDelivery(items);


        // ✅ 5. Вывод результата
        Console.WriteLine("\n📦 Итог по ячейкам:");
        foreach (var cell in warehouse.Cells.Where(c => c.Items.Any()))
        {
            Console.WriteLine($"Ячейка №{cell.Id}: {cell.Items.Count} товаров");
        }

        Console.WriteLine("\n🏁 Работа завершена.");
    }
    
}
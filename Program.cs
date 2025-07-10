class Program
{
    static void Main()
    {
        var deliveryMaker = new DeliveryMaker();
        var warehouseManager = new WarehouseManager();

        // Генерируем поставку (тестовая или из JSON)
        var delivery = deliveryMaker.GenerateDeliveryFromJson("wb_products.json"); // Или GenerateDeliveryFromJson("wb_products.json");

        warehouseManager.StartDelivery(delivery);

        while (true)
        {
            Console.WriteLine("\nВыберите действие: [a] Принять товар, [p] Выдать клиенту, [r] Возобновить, [q] Выход");
            var input = Console.ReadLine()?.ToLower();

            if (input == "q") break;

            if (input == "a")
            {
                warehouseManager.AcceptSingleItem();
            }
            else if (input == "p")
            {
                warehouseManager.InterruptAcceptance();
                warehouseManager.PickupItem(new Random().Next(1, 1000));
                warehouseManager.ResumeAcceptance();
            }
            else if (input == "r")
            {
                warehouseManager.ResumeAcceptance();
            }

            warehouseManager.CheckDeadlines();

            // Вывод состояния ячеек
            Console.WriteLine("\n📦 Состояние ячеек:");
            foreach (var cell in warehouseManager.Cells.Where(c => c.IsFull))
            {
                Console.WriteLine($"Ячейка №{cell.Id}: {cell.Items.Count} товаров");
            }
        }

        Console.WriteLine("\n🏁 Работа завершена.");
    }
}
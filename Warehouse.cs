using System;
using System.Collections.Generic;
using System.Linq;

public class Warehouse
{
    // История всех доставок (список списков товаров)
    public List<List<Item>> Deliveries { get; set; } = new();

    // Все ячейки склада
    public List<Cell> Cells { get; set; }

    // Конструктор склада — сразу создаём 30 пустых ячеек
    public Warehouse()
    {
        Cells = new List<Cell>();

        for (int i = 1; i <= 100; i++)  // 30 ячеек с уникальным Id от 1 до 30
        {
            Cells.Add(new Cell
            {
                Id = i,
                Capacity = 7  // Максимум 5 товаров на ячейку (можешь изменить)
            });
        }
    }

    // Метод приёмки поставки (ручной режим через консоль)
    public void AcceptDelivery(List<Item> delivery)
    {
        Console.WriteLine($"🚚 Новая поставка: {delivery.Count} товаров.");

        var rand = new Random();  // для рандомного выбора ячеек
        int current = 0;          // счётчик принятого товара

        // Проходим по каждому товару в доставке
        foreach (var item in delivery)
        {
            // Показываем информацию по товару
            Console.WriteLine($"\n📦 Принять товар {current + 1}/{delivery.Count}:");
            Console.WriteLine($"Артикул: {item.Article}, Название: {item.Name}");

            // Просим нажать Enter или ввести 'q' для выхода
            Console.Write("Нажмите Enter для приёмки (или 'q' для выхода): ");
            var input = Console.ReadLine();

            // Если пользователь ввёл 'q' — выходим из цикла
            if (input == "q")
            {
                Console.WriteLine("⛔ Приёмка прервана.");
                break;
            }

            // Перемешиваем список ячеек, чтобы выбор был случайным
            var shuffled = Cells.OrderBy(_ => rand.Next()).ToList();

            // Находим первую неполную ячейку из перемешанного списка
            var targetCell = shuffled.FirstOrDefault(c => !c.IsFull);

            // Если все ячейки уже заполнены — выводим ошибку и прерываем приёмку
            if (targetCell == null)
            {
                Console.WriteLine("❌ Все ячейки заполнены!");
                break;
            }

            // Присваиваем товару номер ячейки и статус "принят"
            item.CellId = targetCell.Id;
            item.isAccepted = true;

            // Добавляем товар в список товаров этой ячейки
            targetCell.Items.Add(item);

            // Уведомление о принятии
            Console.WriteLine($"✅ Товар принят. Назначена ячейка №{targetCell.Id}");

            current++;  // увеличиваем счётчик
        }

        // Добавляем эту доставку в историю (для возможного анализа)
        Deliveries.Add(delivery);

        // Общая статистика
        Console.WriteLine($"\n📦 Приемка завершена. Всего принято: {current}");
    }

    // Метод для "забора" всех товаров клиента с указанным clientId
    // Пока не используется — это для будущей логики NPC
    public void PickupItem(int clientId)
    {
        foreach (var cell in Cells)
        {
            // Удаляем товары, чей ClientId совпадает
            cell.Items.RemoveAll(i => i.ClientId == clientId);
        }
    }
}

public class WarehouseManager
{
    public List<Delivery> Deliveries { get; private set; } = new List<Delivery>();
    public List<Cell> Cells { get; private set; } = new List<Cell>();
    private Delivery _currentDelivery;
    private Box _currentBox;
    private int _currentItemIndex;
    private bool _isAccepting;
    private Random _random = new Random();
    private float _rating = 100f;

    public WarehouseManager()
    {
        for (int i = 1; i <= 30; i++)
        {
            Cells.Add(new Cell { Id = i, Capacity = 5 });
        }
    }

    // Начать приёмку поставки
    public void StartDelivery(Delivery delivery)
    {
        Deliveries.Add(delivery);
        _currentDelivery = delivery;
        _currentBox = delivery.Boxes.FirstOrDefault();
        _currentItemIndex = 0;
        _isAccepting = true;

        Console.WriteLine($"🚚 Начата приёмка поставки #{delivery.Id}: {delivery.Boxes.Sum(b => b.Items.Count)} товаров.");
    }

    // Принять один товар
    public void AcceptSingleItem()
    {
        if (!_isAccepting || _currentDelivery == null || _currentBox == null)
        {
            Console.WriteLine("❌ Нет активной поставки.");
            return;
        }

        if (_currentItemIndex >= _currentBox.Items.Count)
        {
            _currentBox = _currentDelivery.Boxes.FirstOrDefault(b => !b.IsFullyAccepted);
            _currentItemIndex = 0;
            if (_currentBox == null)
            {
                _isAccepting = false;
                Console.WriteLine($"✅ Поставка #{_currentDelivery.Id} полностью принята.");
                _currentDelivery = null;
                return;
            }
            Console.WriteLine($"📦 Переход к коробке #{_currentBox.Id}.");
        }

        var item = _currentBox.Items[_currentItemIndex];
        if (item.IsAccepted)
        {
            Console.WriteLine($"Товар {item.Name} (Артикул: {item.Article}) уже принят.");
            _currentItemIndex++;
            return;
        }

        var shuffled = Cells.OrderBy(_ => _random.Next()).ToList();
        var targetCell = shuffled.FirstOrDefault(c => !c.IsFull);

        if (targetCell == null)
        {
            Console.WriteLine("❌ Все ячейки заполнены!");
            _isAccepting = false;
            return;
        }

        item.CellId = targetCell.Id;
        item.IsAccepted = true;
        targetCell.Items.Add(item);

        Console.WriteLine($"✅ Товар {item.Name} (Артикул: {item.Article}) принят в ячейку №{targetCell.Id}");
        _currentItemIndex++;

        if (_currentBox.IsFullyAccepted)
        {
            Console.WriteLine($"📦 Коробка #{_currentBox.Id} полностью принята.");
        }
    }

    // Прервать приёмку
    public void InterruptAcceptance()
    {
        if (_isAccepting)
        {
            _isAccepting = false;
            Console.WriteLine("⛔ Приёмка прервана.");
        }
    }

    // Возобновить приёмку
    public void ResumeAcceptance()
    {
        if (!_isAccepting && _currentDelivery != null && !_currentDelivery.IsCompleted)
        {
            _isAccepting = true;
            _currentBox = _currentDelivery.Boxes.FirstOrDefault(b => !b.IsFullyAccepted);
            _currentItemIndex = _currentBox != null ? _currentBox.Items.FindIndex(i => !i.IsAccepted) : 0;
            Console.WriteLine("🔄 Приёмка возобновлена.");
        }
    }

    // Выдать товары клиенту
    public void PickupItem(int clientId)
    {
        var itemsToRemove = new List<Item>();
        foreach (var cell in Cells)
        {
            var clientItems = cell.Items.Where(i => i.ClientId == clientId).ToList();
            foreach (var item in clientItems)
            {
                itemsToRemove.Add(item);
                Console.WriteLine($"📤 Товар {item.Name} (Артикул: {item.Article}) выдан клиенту {clientId}.");
            }
            cell.Items.RemoveAll(i => i.ClientId == clientId);
        }
        _rating += itemsToRemove.Count * 0.5f; // Бонус за выдачу
        Console.WriteLine($"🏆 Рейтинг: {_rating}");
    }

    // Проверка дедлайнов
    public void CheckDeadlines()
    {
        foreach (var delivery in Deliveries.Where(d => !d.IsCompleted))
        {
            var timeLeft = delivery.Deadline - (DateTime.Now - delivery.CreationTime);
            if (timeLeft <= TimeSpan.Zero)
            {
                Console.WriteLine($"⏰ Дедлайн поставки #{delivery.Id} истёк! Штраф: -10.");
                _rating -= 10f;
                if (delivery == _currentDelivery)
                {
                    _currentDelivery = null;
                    _isAccepting = false;
                }
            }
            else
            {
                Console.WriteLine($"⏳ Поставка #{delivery.Id}: осталось {timeLeft.TotalSeconds:F1} сек.");
            }
        }
    }
}
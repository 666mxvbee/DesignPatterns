using System;
using System.Collections.Generic;

namespace DesignPatterns.Behavioral.Observer
{
    // Аргументы события - что именно изменилось
    public sealed class PriceChangedEventArgs : EventArgs
    {
        public decimal OldPrice { get; }
        public decimal NewPrice { get; }

        public PriceChangedEventArgs(decimal oldPrice, decimal newPrice)
        {
            OldPrice = oldPrice;
            NewPrice = newPrice;
        }
    }

    // Субъект (издатель) - хранит состояние и уведомляет подписчиков об изменениях
    // через стандартный для .NET механизм событий (это и есть Observer, встроенный в язык).
    public sealed class Stock
    {
        public string Symbol { get; }
        private decimal _price;

        public event EventHandler<PriceChangedEventArgs>? PriceChanged;

        public Stock(string symbol, decimal initialPrice)
        {
            Symbol = symbol;
            _price = initialPrice;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice == _price) return;

            var args = new PriceChangedEventArgs(_price, newPrice);
            _price = newPrice;

            // Уведомляем всех подписчиков. Stock ничего не знает о том,
            // кто именно подписан и что они будут делать с этой информацией.
            PriceChanged?.Invoke(this, args);
        }
    }

    // Наблюдатели - независимые друг от друга, каждый реагирует по-своему
    public sealed class PriceLogger
    {
        public void Subscribe(Stock stock) => stock.PriceChanged += OnPriceChanged;

        private void OnPriceChanged(object? sender, PriceChangedEventArgs e)
        {
            var stock = (Stock)sender!;
            Console.WriteLine($"[Лог] {stock.Symbol}: {e.OldPrice} -> {e.NewPrice}");
        }
    }

    public sealed class PriceAlert
    {
        private readonly decimal _threshold;

        public PriceAlert(decimal threshold) => _threshold = threshold;

        public void Subscribe(Stock stock) => stock.PriceChanged += OnPriceChanged;

        private void OnPriceChanged(object? sender, PriceChangedEventArgs e)
        {
            if (e.NewPrice >= _threshold)
            {
                Console.WriteLine($"[Алерт] Цена достигла порога {_threshold}!");
            }
        }
    }

    public static class Demo
    {
        public static void Run()
        {
            var stock = new Stock("ACME", 100m);

            var logger = new PriceLogger();
            var alert = new PriceAlert(threshold: 120m);

            logger.Subscribe(stock);
            alert.Subscribe(stock);

            stock.UpdatePrice(110m);
            stock.UpdatePrice(125m); // сработает и лог, и алерт
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Behavioral.Strategy
{
    // Вариант 1: классическая реализация через интерфейс.
    // Уместна, когда у алгоритма несколько операций или своё внутреннее состояние.
    public interface IDiscountStrategy
    {
        decimal Apply(decimal totalPrice);
    }

    public sealed class NoDiscount : IDiscountStrategy
    {
        public decimal Apply(decimal totalPrice) => totalPrice;
    }

    public sealed class PercentageDiscount : IDiscountStrategy
    {
        private readonly decimal _percent;

        public PercentageDiscount(decimal percent) => _percent = percent;

        public decimal Apply(decimal totalPrice) => totalPrice * (1 - _percent / 100m);
    }

    public sealed class FixedAmountDiscount : IDiscountStrategy
    {
        private readonly decimal _amount;

        public FixedAmountDiscount(decimal amount) => _amount = amount;

        public decimal Apply(decimal totalPrice) => Math.Max(0, totalPrice - _amount);
    }

    // Контекст, использующий стратегию. Он не знает, какая конкретно скидка применяется -
    // это решает вызывающий код, подставляя нужную реализацию.
    public sealed class Order
    {
        private readonly IDiscountStrategy _discountStrategy;
        private readonly decimal _totalPrice;

        public Order(decimal totalPrice, IDiscountStrategy discountStrategy)
        {
            _totalPrice = totalPrice;
            _discountStrategy = discountStrategy;
        }

        public decimal GetFinalPrice() => _discountStrategy.Apply(_totalPrice);
    }

    // Вариант 2: стратегия как делегат - когда алгоритм это одна операция без состояния,
    // в C# зачастую нет смысла заводить под неё интерфейс и класс.
    public static class SortingDemo
    {
        public static IEnumerable<T> SortBy<T>(IEnumerable<T> items, Comparison<T> strategy)
        {
            var list = items.ToList();
            list.Sort(strategy);
            return list;
        }
    }

    public static class Demo
    {
        public static void Run()
        {
            var order1 = new Order(1000m, new PercentageDiscount(10));
            var order2 = new Order(1000m, new FixedAmountDiscount(150));

            Console.WriteLine($"Со скидкой 10%: {order1.GetFinalPrice():C}");
            Console.WriteLine($"Со скидкой 150: {order2.GetFinalPrice():C}");

            // Здесь стратегия сортировки передаётся просто как лямбда (делегат),
            // без отдельного интерфейса ISortStrategy.
            var names = new[] { "Борис", "Анна", "Вера" };
            var sorted = SortingDemo.SortBy(names, (a, b) => string.CompareOrdinal(a, b));
            Console.WriteLine(string.Join(", ", sorted));
        }
    }
}

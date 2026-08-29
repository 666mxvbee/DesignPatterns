using System;

namespace DesignPatterns.Structural.Decorator
{
    // Общий интерфейс для базового объекта и всех декораторов
    public interface ICoffee
    {
        string Describe();
        decimal GetCost();
    }

    // Базовая реализация (то, что декорируем)
    public sealed class Espresso : ICoffee
    {
        public string Describe() => "Эспрессо";
        public decimal GetCost() => 90m;
    }

    // Базовый декоратор хранит ссылку на оборачиваемый объект
    // и по умолчанию просто делегирует ему вызовы.
    public abstract class CoffeeDecorator : ICoffee
    {
        protected readonly ICoffee Inner;

        protected CoffeeDecorator(ICoffee inner)
        {
            Inner = inner;
        }

        public virtual string Describe() => Inner.Describe();
        public virtual decimal GetCost() => Inner.GetCost();
    }

    public sealed class WithMilk : CoffeeDecorator
    {
        public WithMilk(ICoffee inner) : base(inner) { }

        public override string Describe() => $"{Inner.Describe()} + молоко";
        public override decimal GetCost() => Inner.GetCost() + 30m;
    }

    public sealed class WithSyrup : CoffeeDecorator
    {
        private readonly string _flavor;

        public WithSyrup(ICoffee inner, string flavor) : base(inner)
        {
            _flavor = flavor;
        }

        public override string Describe() => $"{Inner.Describe()} + сироп ({_flavor})";
        public override decimal GetCost() => Inner.GetCost() + 20m;
    }

    public static class Demo
    {
        public static void Run()
        {
            // Декораторы можно комбинировать в любом порядке и количестве -
            // каждый добавляет своё поведение поверх предыдущего, ничего не зная об остальных.
            ICoffee order = new WithSyrup(new WithMilk(new Espresso()), "карамель");

            Console.WriteLine($"{order.Describe()} - {order.GetCost():C}");
        }
    }
}

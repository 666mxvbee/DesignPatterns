using System;

namespace DesignPatterns.Behavioral.State
{
    // Контекст хранит ссылку на текущее состояние и делегирует ему операции.
    // Сам контекст не содержит условной логики "если статус такой-то, то...".
    public sealed class OrderContext
    {
        public IOrderState CurrentState { get; private set; }

        public OrderContext()
        {
            CurrentState = new NewOrderState();
        }

        // Позволяет состояниям переключать контекст на следующее состояние
        public void TransitionTo(IOrderState state)
        {
            Console.WriteLine($"Переход: {CurrentState.GetType().Name} -> {state.GetType().Name}");
            CurrentState = state;
        }

        public void Pay() => CurrentState.Pay(this);
        public void Ship() => CurrentState.Ship(this);
        public void Cancel() => CurrentState.Cancel(this);
    }

    // Общий интерфейс состояния - один метод на каждую возможную операцию контекста.
    // Не все операции имеют смысл в каждом состоянии - об этом сообщает сама реализация.
    public interface IOrderState
    {
        void Pay(OrderContext context);
        void Ship(OrderContext context);
        void Cancel(OrderContext context);
    }

    public sealed class NewOrderState : IOrderState
    {
        public void Pay(OrderContext context) => context.TransitionTo(new PaidOrderState());

        public void Ship(OrderContext context) =>
            Console.WriteLine("Нельзя отгрузить неоплаченный заказ");

        public void Cancel(OrderContext context) => context.TransitionTo(new CancelledOrderState());
    }

    public sealed class PaidOrderState : IOrderState
    {
        public void Pay(OrderContext context) =>
            Console.WriteLine("Заказ уже оплачен");

        public void Ship(OrderContext context) => context.TransitionTo(new ShippedOrderState());

        public void Cancel(OrderContext context) => context.TransitionTo(new CancelledOrderState());
    }

    public sealed class ShippedOrderState : IOrderState
    {
        public void Pay(OrderContext context) =>
            Console.WriteLine("Заказ уже отгружен, повторная оплата невозможна");

        public void Ship(OrderContext context) =>
            Console.WriteLine("Заказ уже отгружен");

        public void Cancel(OrderContext context) =>
            Console.WriteLine("Отгруженный заказ отменить нельзя, оформите возврат");
    }

    public sealed class CancelledOrderState : IOrderState
    {
        public void Pay(OrderContext context) =>
            Console.WriteLine("Заказ отменён, оплата невозможна");

        public void Ship(OrderContext context) =>
            Console.WriteLine("Заказ отменён, отгрузка невозможна");

        public void Cancel(OrderContext context) =>
            Console.WriteLine("Заказ уже отменён");
    }

    public static class Demo
    {
        public static void Run()
        {
            var order = new OrderContext();

            order.Ship();  // нельзя - не оплачен
            order.Pay();   // New -> Paid
            order.Ship();  // Paid -> Shipped
            order.Cancel(); // нельзя - уже отгружен
        }
    }
}

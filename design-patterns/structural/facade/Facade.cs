using System;

namespace DesignPatterns.Structural.Facade
{
    // Сложная подсистема из нескольких взаимосвязанных классов.
    // По отдельности каждый класс делает свою узкую задачу и требует знания
    // о порядке вызовов и о том, как передавать данные между ними.

    public sealed class InventoryService
    {
        public bool Reserve(string sku, int quantity)
        {
            Console.WriteLine($"Резервируем {quantity} шт. товара {sku}");
            return true;
        }
    }

    public sealed class PaymentService
    {
        public bool Charge(string customerId, decimal amount)
        {
            Console.WriteLine($"Списываем {amount:C} с клиента {customerId}");
            return true;
        }
    }

    public sealed class ShippingService
    {
        public string ScheduleDelivery(string customerId, string sku)
        {
            Console.WriteLine($"Планируем доставку {sku} клиенту {customerId}");
            return "TRACK-12345";
        }
    }

    public sealed class NotificationService
    {
        public void NotifyOrderCreated(string customerId, string trackingNumber)
        {
            Console.WriteLine($"Отправляем клиенту {customerId} письмо с трек-номером {trackingNumber}");
        }
    }

    // Facade даёт один простой метод для типового сценария "оформить заказ",
    // пряча за собой всю последовательность вызовов подсистемы.
    public sealed class OrderFacade
    {
        private readonly InventoryService _inventory = new();
        private readonly PaymentService _payment = new();
        private readonly ShippingService _shipping = new();
        private readonly NotificationService _notification = new();

        public void PlaceOrder(string customerId, string sku, int quantity, decimal amount)
        {
            if (!_inventory.Reserve(sku, quantity))
            {
                throw new InvalidOperationException("Товара нет в наличии");
            }

            _payment.Charge(customerId, amount);
            string trackingNumber = _shipping.ScheduleDelivery(customerId, sku);
            _notification.NotifyOrderCreated(customerId, trackingNumber);
        }
    }

    public static class Demo
    {
        public static void Run()
        {
            // Клиентскому коду не нужно знать про четыре сервиса и порядок их вызова.
            var facade = new OrderFacade();
            facade.PlaceOrder(customerId: "C-1", sku: "SKU-42", quantity: 1, amount: 19.90m);
        }
    }
}

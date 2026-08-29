using System;

namespace DesignPatterns.Creational.FactoryMethod
{
    // Продукт: общий интерфейс для всех уведомлений
    public interface INotification
    {
        void Send(string message);
    }

    public sealed class EmailNotification : INotification
    {
        public void Send(string message) => Console.WriteLine($"[Email] {message}");
    }

    public sealed class SmsNotification : INotification
    {
        public void Send(string message) => Console.WriteLine($"[SMS] {message}");
    }

    // Создатель: базовый класс задаёт алгоритм работы (Notify),
    // но само создание продукта делегирует подклассам через CreateNotification.
    public abstract class NotificationSender
    {
        // Это и есть Factory Method - точка расширения для подклассов.
        protected abstract INotification CreateNotification();

        // Остальная логика общая для всех отправителей и переиспользуется без дублирования.
        public void Notify(string message)
        {
            INotification notification = CreateNotification();
            Console.WriteLine("Готовим отправку...");
            notification.Send(message);
        }
    }

    public sealed class EmailNotificationSender : NotificationSender
    {
        protected override INotification CreateNotification() => new EmailNotification();
    }

    public sealed class SmsNotificationSender : NotificationSender
    {
        protected override INotification CreateNotification() => new SmsNotification();
    }

    public static class Demo
    {
        public static void Run()
        {
            NotificationSender sender = new EmailNotificationSender();
            sender.Notify("Заказ подтверждён");

            sender = new SmsNotificationSender();
            sender.Notify("Ваш код: 4821");
        }
    }
}

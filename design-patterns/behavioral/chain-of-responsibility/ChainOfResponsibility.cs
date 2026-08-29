using System;

namespace DesignPatterns.Behavioral.ChainOfResponsibility
{
    public sealed class SupportTicket
    {
        public string Description { get; }
        public int SeverityLevel { get; } // 1 - низкий, 3 - критический

        public SupportTicket(string description, int severityLevel)
        {
            Description = description;
            SeverityLevel = severityLevel;
        }
    }

    // Общий интерфейс обработчика - и сам обрабатывает запрос, и хранит ссылку
    // на следующего обработчика в цепочке.
    public abstract class SupportHandler
    {
        private SupportHandler? _next;

        // Возвращаем handler, чтобы можно было строить цепочку одной цепочкой вызовов
        public SupportHandler SetNext(SupportHandler next)
        {
            _next = next;
            return next;
        }

        public void Handle(SupportTicket ticket)
        {
            if (CanHandle(ticket))
            {
                Process(ticket);
                return;
            }

            if (_next is not null)
            {
                _next.Handle(ticket);
            }
            else
            {
                Console.WriteLine($"Тикет «{ticket.Description}» никто не смог обработать");
            }
        }

        protected abstract bool CanHandle(SupportTicket ticket);
        protected abstract void Process(SupportTicket ticket);
    }

    public sealed class FirstLineSupport : SupportHandler
    {
        protected override bool CanHandle(SupportTicket ticket) => ticket.SeverityLevel == 1;

        protected override void Process(SupportTicket ticket) =>
            Console.WriteLine($"[Первая линия] Обработан: {ticket.Description}");
    }

    public sealed class SecondLineSupport : SupportHandler
    {
        protected override bool CanHandle(SupportTicket ticket) => ticket.SeverityLevel == 2;

        protected override void Process(SupportTicket ticket) =>
            Console.WriteLine($"[Вторая линия] Обработан: {ticket.Description}");
    }

    public sealed class EngineeringTeam : SupportHandler
    {
        protected override bool CanHandle(SupportTicket ticket) => ticket.SeverityLevel >= 3;

        protected override void Process(SupportTicket ticket) =>
            Console.WriteLine($"[Инженеры] Обработан критический тикет: {ticket.Description}");
    }

    public static class Demo
    {
        public static void Run()
        {
            var firstLine = new FirstLineSupport();
            var secondLine = new SecondLineSupport();
            var engineering = new EngineeringTeam();

            // Строим цепочку: первая линия -> вторая линия -> инженеры
            firstLine.SetNext(secondLine).SetNext(engineering);

            firstLine.Handle(new SupportTicket("Не приходит письмо", severityLevel: 1));
            firstLine.Handle(new SupportTicket("Ошибка в отчёте", severityLevel: 2));
            firstLine.Handle(new SupportTicket("Сервис недоступен", severityLevel: 3));
        }
    }
}

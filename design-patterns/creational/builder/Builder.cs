using System;
using System.Collections.Generic;

namespace DesignPatterns.Creational.Builder
{
    // Продукт - сложный неизменяемый объект со множеством опциональных частей
    public sealed class Report
    {
        public string Title { get; }
        public IReadOnlyList<string> Sections { get; }
        public bool HasSummary { get; }

        public Report(string title, IReadOnlyList<string> sections, bool hasSummary)
        {
            Title = title;
            Sections = sections;
            HasSummary = hasSummary;
        }

        public override string ToString() =>
            $"Отчёт «{Title}», разделов: {Sections.Count}, есть резюме: {HasSummary}";
    }

    // Строитель с fluent-интерфейсом: каждый метод возвращает this,
    // что позволяет собирать объект цепочкой вызовов.
    public sealed class ReportBuilder
    {
        private string _title = "Без названия";
        private readonly List<string> _sections = new();
        private bool _hasSummary;

        public ReportBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public ReportBuilder AddSection(string section)
        {
            _sections.Add(section);
            return this;
        }

        public ReportBuilder WithSummary()
        {
            _hasSummary = true;
            return this;
        }

        // Финальный шаг - собираем неизменяемый продукт из накопленного состояния.
        public Report Build() => new(_title, _sections.AsReadOnly(), _hasSummary);
    }

    public static class Demo
    {
        public static void Run()
        {
            Report report = new ReportBuilder()
                .WithTitle("Продажи за квартал")
                .AddSection("Выручка по регионам")
                .AddSection("Топ-10 клиентов")
                .WithSummary()
                .Build();

            Console.WriteLine(report);
        }
    }
}

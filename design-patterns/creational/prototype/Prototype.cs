using System;
using System.Collections.Generic;

namespace DesignPatterns.Creational.Prototype
{
    public interface IPrototype<out T>
    {
        T Clone();
    }

    // Конкретный прототип сам копирует своё состояние, включая закрытые поля.
    public sealed class Document : IPrototype<Document>
    {
        private readonly List<string> _pages;

        public string Title { get; }
        public IReadOnlyList<string> Pages => _pages.AsReadOnly();

        public Document(string title, IEnumerable<string> pages)
        {
            Title = title;
            _pages = new List<string>(pages);
        }

        private Document(Document source)
        {
            Title = source.Title;
            // Глубокая копия изменяемой коллекции: клон не делит список с оригиналом.
            _pages = new List<string>(source._pages);
        }

        public Document Clone() => new(this);

        public void AddPage(string text) => _pages.Add(text);
    }

    // Необязательное хранилище заранее настроенных прототипов.
    public sealed class DocumentRegistry
    {
        private readonly Dictionary<string, Document> _prototypes = new();

        public void Register(string key, Document prototype) =>
            _prototypes[key] = prototype;

        public Document Create(string key) =>
            _prototypes.TryGetValue(key, out Document? prototype)
                ? prototype.Clone()
                : throw new KeyNotFoundException($"Прототип '{key}' не зарегистрирован.");
    }

    public static class Demo
    {
        public static void Run()
        {
            var registry = new DocumentRegistry();
            registry.Register("invoice", new Document(
                "Счёт",
                new[] { "Реквизиты", "Позиции", "Итого" }));

            Document first = registry.Create("invoice");
            Document second = registry.Create("invoice");
            first.AddPage("Комментарий для первого клиента");

            Console.WriteLine(first.Pages.Count);  // 4
            Console.WriteLine(second.Pages.Count); // 3: коллекция скопирована глубоко
        }
    }
}

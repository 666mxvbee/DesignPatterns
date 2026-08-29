using System;

namespace DesignPatterns.Behavioral.TemplateMethod
{
    // Базовый класс задаёт общий скелет алгоритма импорта данных.
    // Порядок шагов и общая логика (открыть, разобрать, сохранить, закрыть) не меняются,
    // а вот "разбор" данных отличается для каждого формата.
    public abstract class DataImporter
    {
        // Это и есть шаблонный метод: он не виртуальный, чтобы подклассы
        // не могли случайно изменить порядок или пропустить шаги.
        public void Import(string filePath)
        {
            Console.WriteLine($"Открываем файл {filePath}");
            string rawData = ReadFile(filePath);

            var records = Parse(rawData); // <- переменный шаг, реализуется подклассами

            Validate(records);
            Save(records);

            Console.WriteLine("Импорт завершён");
        }

        protected virtual string ReadFile(string filePath) => $"raw-content-of({filePath})";

        // Абстрактный шаг - обязателен для переопределения
        protected abstract string[] Parse(string rawData);

        // "Хук" - шаг с реализацией по умолчанию, который подкласс может переопределить,
        // но не обязан этого делать.
        protected virtual void Validate(string[] records)
        {
            Console.WriteLine($"Базовая проверка: записей {records.Length}");
        }

        protected virtual void Save(string[] records)
        {
            Console.WriteLine($"Сохраняем {records.Length} записей в базу");
        }
    }

    public sealed class CsvImporter : DataImporter
    {
        protected override string[] Parse(string rawData) =>
            rawData.Split(',');
    }

    public sealed class JsonImporter : DataImporter
    {
        protected override string[] Parse(string rawData) =>
            new[] { rawData }; // упрощённо, для примера

        // Переопределяем хук - для JSON нужна дополнительная проверка схемы
        protected override void Validate(string[] records)
        {
            base.Validate(records);
            Console.WriteLine("Дополнительно проверяем JSON-схему");
        }
    }

    public static class Demo
    {
        public static void Run()
        {
            DataImporter importer = new CsvImporter();
            importer.Import("data.csv");

            Console.WriteLine();

            importer = new JsonImporter();
            importer.Import("data.json");
        }
    }
}

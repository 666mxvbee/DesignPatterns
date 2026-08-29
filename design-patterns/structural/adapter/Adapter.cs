using System;

namespace DesignPatterns.Structural.Adapter
{
    // Интерфейс, который ожидает наш код (Target)
    public interface IJsonLogger
    {
        void LogAsJson(string json);
    }

    // Существующий сторонний класс с несовместимым интерфейсом (Adaptee).
    // Представим, что это код из NuGet-пакета, который мы не можем менять.
    public sealed class LegacyXmlLogger
    {
        public void WriteXml(string xml) => Console.WriteLine($"[XML LOG] {xml}");
    }

    // Adapter преобразует вызов IJsonLogger.LogAsJson в вызов LegacyXmlLogger.WriteXml,
    // конвертируя формат данных внутри.
    public sealed class XmlLoggerAdapter : IJsonLogger
    {
        private readonly LegacyXmlLogger _legacyLogger;

        public XmlLoggerAdapter(LegacyXmlLogger legacyLogger)
        {
            _legacyLogger = legacyLogger;
        }

        public void LogAsJson(string json)
        {
            string xml = ConvertJsonToXml(json);
            _legacyLogger.WriteXml(xml);
        }

        // Упрощённая "конвертация" для примера
        private static string ConvertJsonToXml(string json) =>
            $"<log>{json}</log>";
    }

    public static class Demo
    {
        // Клиентский код работает только с IJsonLogger и ничего не знает
        // о существовании LegacyXmlLogger.
        private static void Report(IJsonLogger logger, string json) => logger.LogAsJson(json);

        public static void Run()
        {
            IJsonLogger logger = new XmlLoggerAdapter(new LegacyXmlLogger());
            Report(logger, "{\"event\":\"OrderCreated\"}");
        }
    }
}

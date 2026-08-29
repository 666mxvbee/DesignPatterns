using System;
using System.Threading;

namespace DesignPatterns.Creational.Singleton
{
    /// <summary>
    /// Классический потокобезопасный Singleton на основе Lazy&lt;T&gt;.
    /// Lazy&lt;T&gt; сам по себе уже реализует ленивую и потокобезопасную инициализацию,
    /// поэтому вручную писать double-checked locking в 99% случаев не требуется.
    /// </summary>
    public sealed class AppSettings
    {
        private static readonly Lazy<AppSettings> LazyInstance =
            new(() => new AppSettings(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static AppSettings Instance => LazyInstance.Value;

        public string ConnectionString { get; }

        // Приватный конструктор - никто, кроме самого класса, не может создать экземпляр через new.
        private AppSettings()
        {
            // Здесь могла бы быть тяжёлая инициализация: чтение файла конфигурации,
            // разбор переменных окружения и т.д. Она выполнится один раз, при первом обращении.
            ConnectionString = "Host=localhost;Database=demo";
        }
    }

    /// <summary>
    /// То же самое, но через статический конструктор - альтернативный вариант,
    /// который CLR гарантированно выполняет один раз и потокобезопасно ещё до первого
    /// обращения к любому статическому члену типа.
    /// </summary>
    public sealed class FeatureFlags
    {
        // Статический конструктор выполняется лениво (при первом обращении к типу)
        // и потокобезопасно - это гарантирует сам CLR.
        private static readonly FeatureFlags InstanceField = new();

        public static FeatureFlags Instance => InstanceField;

        public bool NewCheckoutEnabled { get; }

        private FeatureFlags()
        {
            NewCheckoutEnabled = true;
        }
    }

    public static class Demo
    {
        public static void Run()
        {
            var s1 = AppSettings.Instance;
            var s2 = AppSettings.Instance;

            Console.WriteLine(ReferenceEquals(s1, s2)); // True - это один и тот же объект
            Console.WriteLine(s1.ConnectionString);

            Console.WriteLine(FeatureFlags.Instance.NewCheckoutEnabled);
        }
    }
}

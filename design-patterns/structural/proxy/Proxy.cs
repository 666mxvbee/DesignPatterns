using System;
using System.Collections.Generic;

namespace DesignPatterns.Structural.Proxy
{
    // Общий интерфейс реального объекта и прокси - клиент не различает их
    public interface IImage
    {
        void Display();
    }

    // Реальный объект - "дорогой" в создании (например, загрузка большого файла с диска)
    public sealed class HighResolutionImage : IImage
    {
        private readonly string _path;

        public HighResolutionImage(string path)
        {
            _path = path;
            LoadFromDisk();
        }

        private void LoadFromDisk() =>
            Console.WriteLine($"Загружаем тяжёлое изображение {_path} с диска...");

        public void Display() =>
            Console.WriteLine($"Отображаем {_path}");
    }

    // Proxy откладывает создание реального объекта до первого реального обращения (lazy loading)
    public sealed class LazyImageProxy : IImage
    {
        private readonly string _path;
        private HighResolutionImage? _realImage;

        public LazyImageProxy(string path)
        {
            _path = path;
        }

        public void Display()
        {
            // Реальный объект создаётся только тогда, когда он действительно нужен
            _realImage ??= new HighResolutionImage(_path);
            _realImage.Display();
        }
    }

    // Ещё один вид Proxy - кеширующий, добавляет проверку кеша перед реальным вызовом
    public sealed class CachingImageProxy : IImage
    {
        private static readonly Dictionary<string, IImage> Cache = new();
        private readonly string _path;

        public CachingImageProxy(string path)
        {
            _path = path;
        }

        public void Display()
        {
            if (!Cache.TryGetValue(_path, out var image))
            {
                image = new HighResolutionImage(_path);
                Cache[_path] = image;
            }
            else
            {
                Console.WriteLine($"Берём {_path} из кеша, повторная загрузка не нужна");
            }

            image.Display();
        }
    }

    public static class Demo
    {
        public static void Run()
        {
            Console.WriteLine("-- Lazy proxy --");
            IImage lazy = new LazyImageProxy("photo.png");
            Console.WriteLine("Прокси создан, изображение ещё не загружено");
            lazy.Display(); // Загрузка произойдёт только сейчас

            Console.WriteLine("-- Caching proxy --");
            IImage cached1 = new CachingImageProxy("banner.png");
            IImage cached2 = new CachingImageProxy("banner.png");
            cached1.Display(); // Реальная загрузка
            cached2.Display(); // Из кеша
        }
    }
}

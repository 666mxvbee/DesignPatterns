using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Structural.Composite
{
    // Общий интерфейс и для листьев (файлов), и для составных узлов (папок) -
    // клиент работает с деревом единообразно, не различая типы узлов.
    public interface IFileSystemEntry
    {
        string Name { get; }
        long GetSize();
        void Print(int indent = 0);
    }

    // Лист - не имеет дочерних элементов
    public sealed class FileEntry : IFileSystemEntry
    {
        public string Name { get; }
        private readonly long _size;

        public FileEntry(string name, long size)
        {
            Name = name;
            _size = size;
        }

        public long GetSize() => _size;

        public void Print(int indent = 0) =>
            Console.WriteLine(new string(' ', indent) + $"- {Name} ({_size} байт)");
    }

    // Составной узел - хранит коллекцию дочерних элементов,
    // которые сами могут быть либо листьями, либо другими составными узлами.
    public sealed class FolderEntry : IFileSystemEntry
    {
        public string Name { get; }
        private readonly List<IFileSystemEntry> _children = new();

        public FolderEntry(string name)
        {
            Name = name;
        }

        public void Add(IFileSystemEntry entry) => _children.Add(entry);

        // Размер папки - рекурсивная сумма размеров всех дочерних элементов.
        // Вызывающему коду не важно, лист это или снова папка.
        public long GetSize() => _children.Sum(c => c.GetSize());

        public void Print(int indent = 0)
        {
            Console.WriteLine(new string(' ', indent) + $"+ {Name}/ ({GetSize()} байт)");
            foreach (var child in _children)
            {
                child.Print(indent + 2);
            }
        }
    }

    public static class Demo
    {
        public static void Run()
        {
            var root = new FolderEntry("project");
            var src = new FolderEntry("src");
            src.Add(new FileEntry("Program.cs", 1200));
            src.Add(new FileEntry("Utils.cs", 800));

            var docs = new FolderEntry("docs");
            docs.Add(new FileEntry("README.md", 500));

            root.Add(src);
            root.Add(docs);
            root.Add(new FileEntry(".gitignore", 50));

            // Клиент вызывает Print/GetSize на корне, не зная заранее,
            // сколько уровней вложенности внутри.
            root.Print();
        }
    }
}

using System;
using System.Collections.Generic;

namespace DesignPatterns.Structural.Flyweight
{
    // Легковес хранит только разделяемое внутреннее состояние и неизменяем.
    public sealed class TreeType
    {
        public TreeType(string name, string color, string texture)
        {
            Name = name;
            Color = color;
            Texture = texture;
        }

        public string Name { get; }
        public string Color { get; }
        public string Texture { get; }

        public string Draw(int x, int y) =>
            $"{Name} ({Color}, {Texture}) в точке ({x}, {y})";
    }

    public sealed class TreeTypeFactory
    {
        private readonly Dictionary<(string Name, string Color, string Texture), TreeType>
            _types = new();

        public TreeType Get(string name, string color, string texture)
        {
            var key = (name, color, texture);
            if (!_types.TryGetValue(key, out TreeType? type))
            {
                type = new TreeType(name, color, texture);
                _types.Add(key, type);
            }

            return type;
        }

        public int Count => _types.Count;
    }

    // Контекст хранит уникальное внешнее состояние и ссылку на общий легковес.
    public sealed class Tree
    {
        public Tree(int x, int y, TreeType type)
        {
            X = x;
            Y = y;
            Type = type;
        }

        public int X { get; }
        public int Y { get; }
        public TreeType Type { get; }
        public string Draw() => Type.Draw(X, Y);
    }

    public static class Demo
    {
        public static void Run()
        {
            var factory = new TreeTypeFactory();
            var forest = new List<Tree>
            {
                new(10, 20, factory.Get("Берёза", "белая", "birch.png")),
                new(30, 40, factory.Get("Берёза", "белая", "birch.png")),
                new(50, 60, factory.Get("Сосна", "зелёная", "pine.png"))
            };

            foreach (Tree tree in forest)
                Console.WriteLine(tree.Draw());

            Console.WriteLine($"Деревьев: {forest.Count}, общих типов: {factory.Count}");
            Console.WriteLine(ReferenceEquals(forest[0].Type, forest[1].Type)); // True
        }
    }
}

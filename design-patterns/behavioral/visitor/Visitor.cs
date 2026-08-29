using System;
using System.Collections.Generic;

namespace DesignPatterns.Behavioral.Visitor
{
    // Иерархия классов фигур - стабильная, новые виды фигур добавляются редко,
    // зато операции над ними (площадь, отрисовка, экспорт) добавляются часто.
    public interface IShape
    {
        // Каждая фигура умеет "принять" посетителя и передать себя в нужный метод
        void Accept(IShapeVisitor visitor);
    }

    public sealed class Circle : IShape
    {
        public double Radius { get; }
        public Circle(double radius) => Radius = radius;

        public void Accept(IShapeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class Rectangle : IShape
    {
        public double Width { get; }
        public double Height { get; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public void Accept(IShapeVisitor visitor) => visitor.Visit(this);
    }

    // Интерфейс посетителя содержит по методу на каждый конкретный тип фигуры -
    // это позволяет обойтись без приведения типов (as/is) внутри операций.
    public interface IShapeVisitor
    {
        void Visit(Circle circle);
        void Visit(Rectangle rectangle);
    }

    // Конкретный посетитель добавляет новую операцию - подсчёт площади -
    // не изменяя ни один из классов фигур.
    public sealed class AreaCalculator : IShapeVisitor
    {
        public double TotalArea { get; private set; }

        public void Visit(Circle circle) => TotalArea += Math.PI * circle.Radius * circle.Radius;

        public void Visit(Rectangle rectangle) => TotalArea += rectangle.Width * rectangle.Height;
    }

    // Ещё одна операция - экспорт в текстовое описание, снова без изменения классов фигур
    public sealed class ShapeDescriptionExporter : IShapeVisitor
    {
        private readonly List<string> _lines = new();

        public void Visit(Circle circle) => _lines.Add($"Круг радиусом {circle.Radius}");

        public void Visit(Rectangle rectangle) => _lines.Add($"Прямоугольник {rectangle.Width}x{rectangle.Height}");

        public string Export() => string.Join(Environment.NewLine, _lines);
    }

    public static class Demo
    {
        public static void Run()
        {
            var shapes = new List<IShape>
            {
                new Circle(radius: 3),
                new Rectangle(width: 4, height: 5),
            };

            var areaCalculator = new AreaCalculator();
            var exporter = new ShapeDescriptionExporter();

            foreach (var shape in shapes)
            {
                shape.Accept(areaCalculator);
                shape.Accept(exporter);
            }

            Console.WriteLine($"Суммарная площадь: {areaCalculator.TotalArea:F2}");
            Console.WriteLine(exporter.Export());
        }
    }
}

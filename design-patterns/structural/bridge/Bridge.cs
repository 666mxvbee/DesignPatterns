using System;

namespace DesignPatterns.Structural.Bridge
{
    // Реализация: низкоуровневые операции конкретной платформы вывода.
    public interface IRenderer
    {
        string RenderCircle(double radius);
    }

    public sealed class VectorRenderer : IRenderer
    {
        public string RenderCircle(double radius) =>
            $"Векторный круг радиуса {radius}";
    }

    public sealed class RasterRenderer : IRenderer
    {
        public string RenderCircle(double radius) =>
            $"Растровый круг радиуса {radius} из пикселей";
    }

    // Абстракция развивается независимо от иерархии реализаций.
    public abstract class Shape
    {
        protected Shape(IRenderer renderer) => Renderer = renderer;

        protected IRenderer Renderer { get; }
        public abstract string Draw();
    }

    public class Circle : Shape
    {
        public Circle(IRenderer renderer, double radius) : base(renderer) =>
            Radius = radius;

        public double Radius { get; }
        public override string Draw() => Renderer.RenderCircle(Radius);
    }

    // Расширенная абстракция не требует новых RasterCircle/VectorCircle.
    public sealed class LabeledCircle : Circle
    {
        public LabeledCircle(IRenderer renderer, double radius, string label)
            : base(renderer, radius) => Label = label;

        public string Label { get; }
        public override string Draw() => $"{Label}: {base.Draw()}";
    }

    public static class Demo
    {
        public static void Run()
        {
            Shape vector = new Circle(new VectorRenderer(), 10);
            Shape rasterWithLabel = new LabeledCircle(new RasterRenderer(), 5, "Аватар");

            Console.WriteLine(vector.Draw());
            Console.WriteLine(rasterWithLabel.Draw());
        }
    }
}

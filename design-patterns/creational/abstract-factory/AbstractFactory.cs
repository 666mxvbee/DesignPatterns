using System;

namespace DesignPatterns.Creational.AbstractFactory
{
    // Семейство продуктов №1
    public interface IButton
    {
        void Render();
    }

    // Семейство продуктов №2
    public interface ICheckbox
    {
        void Render();
    }

    // Конкретные продукты для темы "Light"
    public sealed class LightButton : IButton
    {
        public void Render() => Console.WriteLine("Светлая кнопка");
    }

    public sealed class LightCheckbox : ICheckbox
    {
        public void Render() => Console.WriteLine("Светлый чекбокс");
    }

    // Конкретные продукты для темы "Dark"
    public sealed class DarkButton : IButton
    {
        public void Render() => Console.WriteLine("Тёмная кнопка");
    }

    public sealed class DarkCheckbox : ICheckbox
    {
        public void Render() => Console.WriteLine("Тёмный чекбокс");
    }

    // Абстрактная фабрика - гарантирует, что кнопка и чекбокс будут из одной темы
    public interface IUiFactory
    {
        IButton CreateButton();
        ICheckbox CreateCheckbox();
    }

    public sealed class LightUiFactory : IUiFactory
    {
        public IButton CreateButton() => new LightButton();
        public ICheckbox CreateCheckbox() => new LightCheckbox();
    }

    public sealed class DarkUiFactory : IUiFactory
    {
        public IButton CreateButton() => new DarkButton();
        public ICheckbox CreateCheckbox() => new DarkCheckbox();
    }

    // Клиентский код работает только с абстракциями и никогда не смешивает
    // компоненты из разных семейств (например, светлую кнопку с тёмным чекбоксом).
    public sealed class SettingsDialog
    {
        private readonly IButton _button;
        private readonly ICheckbox _checkbox;

        public SettingsDialog(IUiFactory factory)
        {
            _button = factory.CreateButton();
            _checkbox = factory.CreateCheckbox();
        }

        public void Render()
        {
            _button.Render();
            _checkbox.Render();
        }
    }

    public static class Demo
    {
        public static void Run()
        {
            IUiFactory factory = DateTime.Now.Hour < 18
                ? new LightUiFactory()
                : new DarkUiFactory();

            var dialog = new SettingsDialog(factory);
            dialog.Render();
        }
    }
}

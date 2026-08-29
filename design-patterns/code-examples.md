# Примеры на C#

Каждая глава сопровождается самостоятельным примером. Файлы используют отдельные пространства имён и содержат метод `Demo.Run()`, поэтому их можно изучать по одному или подключить в общий учебный проект.

## Порождающие паттерны

| Паттерн | Исходный код |
|---|---|
| Factory Method | [`FactoryMethod.cs`](creational/factory-method/FactoryMethod-code.md) |
| Abstract Factory | [`AbstractFactory.cs`](creational/abstract-factory/AbstractFactory-code.md) |
| Builder | [`Builder.cs`](creational/builder/Builder-code.md) · [`BuilderVariants.cs`](creational/builder/BuilderVariants-code.md) |
| Prototype | [`Prototype.cs`](creational/prototype/Prototype-code.md) |
| Singleton | [`Singleton.cs`](creational/singleton/Singleton-code.md) |

## Структурные паттерны

| Паттерн | Исходный код |
|---|---|
| Adapter | [`Adapter.cs`](structural/adapter/Adapter-code.md) |
| Bridge | [`Bridge.cs`](structural/bridge/Bridge-code.md) |
| Composite | [`Composite.cs`](structural/composite/Composite-code.md) |
| Decorator | [`Decorator.cs`](structural/decorator/Decorator-code.md) |
| Facade | [`Facade.cs`](structural/facade/Facade-code.md) |
| Flyweight | [`Flyweight.cs`](structural/flyweight/Flyweight-code.md) |
| Proxy | [`Proxy.cs`](structural/proxy/Proxy-code.md) |

## Поведенческие паттерны

| Паттерн | Исходный код |
|---|---|
| Chain of Responsibility | [`ChainOfResponsibility.cs`](behavioral/chain-of-responsibility/ChainOfResponsibility-code.md) |
| Command | [`Command.cs`](behavioral/command/Command-code.md) |
| Iterator | [`Iterator.cs`](behavioral/iterator/Iterator-code.md) |
| Mediator | [`Mediator.cs`](behavioral/mediator/Mediator-code.md) |
| Memento | [`Memento.cs`](behavioral/memento/Memento-code.md) |
| Observer | [`Observer.cs`](behavioral/observer/Observer-code.md) |
| State | [`State.cs`](behavioral/state/State-code.md) |
| Strategy | [`Strategy.cs`](behavioral/strategy/Strategy-code.md) |
| Template Method | [`TemplateMethod.cs`](behavioral/template-method/TemplateMethod-code.md) |
| Visitor | [`Visitor.cs`](behavioral/visitor/Visitor-code.md) |

!!! tip "Как запускать"

    Создайте консольный проект, добавьте нужный `.cs`-файл и вызовите соответствующий `Demo.Run()` из `Main`. Все примеры рассчитаны на современный C# и не требуют сторонних пакетов.

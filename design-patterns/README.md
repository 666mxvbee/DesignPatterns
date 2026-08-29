# Паттерны проектирования на C# / .NET

## Оглавление

### Порождающие паттерны

| Паттерн | Описание |
|---|---|
| [Singleton](./creational/singleton/README.md) | Гарантирует единственный экземпляр класса и глобальную точку доступа к нему |
| [Factory Method](./creational/factory-method/README.md) | Делегирует создание объекта подклассам через переопределяемый метод |
| [Abstract Factory](./creational/abstract-factory/README.md) | Создаёт семейства связанных объектов без указания конкретных классов |
| [Builder](./creational/builder/README.md) | Пошагово собирает сложный объект, отделяя конструирование от представления |
| [Prototype](./creational/prototype/README.md) | Создаёт новые объекты копированием заранее настроенных прототипов |

### Структурные паттерны

| Паттерн | Описание |
|---|---|
| [Adapter](./structural/adapter/README.md) | Преобразует интерфейс одного класса в интерфейс, ожидаемый клиентом |
| [Bridge](./structural/bridge/README.md) | Разделяет абстракцию и реализацию на независимо развиваемые иерархии |
| [Facade](./structural/facade/README.md) | Даёт простой унифицированный интерфейс к сложной подсистеме |
| [Decorator](./structural/decorator/README.md) | Динамически добавляет объекту новое поведение, оборачивая его |
| [Composite](./structural/composite/README.md) | Компонует объекты в древовидные структуры и работает с ними единообразно |
| [Flyweight](./structural/flyweight/README.md) | Экономит память, разделяя повторяющееся состояние множества объектов |
| [Proxy](./structural/proxy/README.md) | Подставляет объект-заместитель, контролирующий доступ к другому объекту |

### Паттерны поведения

| Паттерн | Описание |
|---|---|
| [Strategy](./behavioral/strategy/README.md) | Инкапсулирует семейство взаимозаменяемых алгоритмов |
| [Template Method](./behavioral/template-method/README.md) | Задаёт скелет алгоритма, отдавая отдельные шаги подклассам |
| [Mediator](./behavioral/mediator/README.md) | Инкапсулирует взаимодействие множества объектов в отдельном посреднике |
| [Iterator](./behavioral/iterator/README.md) | Даёт последовательный доступ к элементам коллекции без раскрытия её структуры |
| [Observer](./behavioral/observer/README.md) | Оповещает зависимые объекты об изменениях состояния субъекта |
| [Memento](./behavioral/memento/README.md) | Сохраняет и восстанавливает состояние объекта без нарушения инкапсуляции |
| [Visitor](./behavioral/visitor/README.md) | Добавляет новые операции над структурой объектов без изменения самих классов |
| [Command](./behavioral/command/README.md) | Превращает запрос в самостоятельный объект с возможностью отмены или очереди |
| [State](./behavioral/state/README.md) | Меняет поведение объекта при изменении его внутреннего состояния |
| [Chain of Responsibility](./behavioral/chain-of-responsibility/README.md) | Передаёт запрос по цепочке обработчиков, пока один из них не обработает его |

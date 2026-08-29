using System;
using System.Collections.Generic;

namespace DesignPatterns.Behavioral.Command
{
    // Получатель - объект, который реально выполняет действия.
    // Он ничего не знает о существовании команд или истории отмен.
    public sealed class TextDocument
    {
        public string Content { get; private set; } = string.Empty;

        public void InsertText(string text, int position) =>
            Content = Content.Insert(position, text);

        public void RemoveText(int position, int length) =>
            Content = Content.Remove(position, length);
    }

    // Общий интерфейс команды - каждое действие умеет выполниться и отмениться
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    // Конкретная команда хранит всё, что нужно для выполнения и отмены действия:
    // ссылку на получателя и параметры операции.
    public sealed class InsertTextCommand : ICommand
    {
        private readonly TextDocument _document;
        private readonly string _text;
        private readonly int _position;

        public InsertTextCommand(TextDocument document, string text, int position)
        {
            _document = document;
            _text = text;
            _position = position;
        }

        public void Execute() => _document.InsertText(_text, _position);

        public void Undo() => _document.RemoveText(_position, _text.Length);
    }

    // Инициатор - хранит историю выполненных команд и умеет их отменять.
    // Он работает только с абстракцией ICommand, не зная о конкретных действиях.
    public sealed class CommandHistory
    {
        private readonly Stack<ICommand> _history = new();

        public void Execute(ICommand command)
        {
            command.Execute();
            _history.Push(command);
        }

        public void UndoLast()
        {
            if (_history.Count == 0)
            {
                Console.WriteLine("Нечего отменять");
                return;
            }

            ICommand last = _history.Pop();
            last.Undo();
        }
    }

    public static class Demo
    {
        public static void Run()
        {
            var document = new TextDocument();
            var history = new CommandHistory();

            history.Execute(new InsertTextCommand(document, "Привет, ", position: 0));
            history.Execute(new InsertTextCommand(document, "мир!", position: 8));
            Console.WriteLine(document.Content); // "Привет, мир!"

            history.UndoLast();
            Console.WriteLine(document.Content); // "Привет, "

            history.UndoLast();
            Console.WriteLine($"«{document.Content}»"); // ""
        }
    }
}

using System;
using System.Collections.Generic;

namespace DesignPatterns.Behavioral.Memento
{
    // Ограниченный контракт: хранитель может держать снимок, но не читать состояние.
    public interface IEditorMemento
    {
        DateTime CreatedAt { get; }
    }

    // Originator - единственный класс, который знает внутреннее устройство снимка.
    public sealed class TextEditor
    {
        private string _text = string.Empty;
        private int _cursor;

        public void SetState(string text, int cursor)
        {
            _text = text;
            _cursor = cursor;
        }

        public string Describe() => $"'{_text}', курсор: {_cursor}";

        public IEditorMemento Save() => new EditorMemento(_text, _cursor);

        public void Restore(IEditorMemento memento)
        {
            if (memento is not EditorMemento snapshot)
                throw new ArgumentException("Снимок создан другим объектом.", nameof(memento));

            _text = snapshot.Text;
            _cursor = snapshot.Cursor;
        }

        private sealed class EditorMemento : IEditorMemento
        {
            public EditorMemento(string text, int cursor)
            {
                Text = text;
                Cursor = cursor;
                CreatedAt = DateTime.UtcNow;
            }

            public string Text { get; }
            public int Cursor { get; }
            public DateTime CreatedAt { get; }
        }
    }

    // Caretaker - управляет историей, не раскрывая содержимое снимков.
    public sealed class EditorHistory
    {
        private readonly Stack<IEditorMemento> _undo = new();

        public void Backup(TextEditor editor) => _undo.Push(editor.Save());

        public bool Undo(TextEditor editor)
        {
            if (_undo.Count == 0)
                return false;

            editor.Restore(_undo.Pop());
            return true;
        }
    }

    public static class Demo
    {
        public static void Run()
        {
            var editor = new TextEditor();
            var history = new EditorHistory();

            editor.SetState("Первая версия", 13);
            history.Backup(editor);
            editor.SetState("Вторая версия", 6);

            Console.WriteLine(editor.Describe());
            history.Undo(editor);
            Console.WriteLine(editor.Describe());
        }
    }
}

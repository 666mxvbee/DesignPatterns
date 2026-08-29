using System;
using System.Collections;
using System.Collections.Generic;

namespace DesignPatterns.Behavioral.Iterator
{
    // Собственная коллекция со скрытой внутренней структурой (кольцевой буфер).
    // Реализуя IEnumerable<T>, мы даём клиенту единый способ обхода (foreach),
    // не раскрывая, что внутри на самом деле массив фиксированного размера с "головой" и "хвостом".
    public sealed class RingBuffer<T> : IEnumerable<T>
    {
        private readonly T[] _items;
        private int _head;
        private int _count;

        public RingBuffer(int capacity)
        {
            _items = new T[capacity];
        }

        public void Add(T item)
        {
            int index = (_head + _count) % _items.Length;
            _items[index] = item;

            if (_count < _items.Length)
            {
                _count++;
            }
            else
            {
                _head = (_head + 1) % _items.Length; // самый старый элемент затирается
            }
        }

        // Благодаря yield return компилятор сам генерирует класс-итератор,
        // реализующий IEnumerator<T> - вручную его писать не нужно.
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                int index = (_head + i) % _items.Length;
                yield return _items[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class Demo
    {
        public static void Run()
        {
            var buffer = new RingBuffer<int>(capacity: 3);
            buffer.Add(1);
            buffer.Add(2);
            buffer.Add(3);
            buffer.Add(4); // 1 будет вытеснен

            // Клиентский код использует обычный foreach и ничего не знает
            // про кольцевой буфер, "голову" и "хвост" внутри.
            foreach (int item in buffer)
            {
                Console.WriteLine(item);
            }
        }
    }
}

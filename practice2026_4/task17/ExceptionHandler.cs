using System;
using System.Collections.Concurrent;

namespace task17
{
    public class ExceptionHandler
    {
        public ConcurrentBag<(ICommand Command, Exception Exception)> Errors { get; } = new();

        public void Handle(ICommand command, Exception exception)
        {
            Errors.Add((command, exception));
            Console.Error.WriteLine($"Ошибка в команде {command.GetType().Name}: {exception.Message}");
        }
    }

}

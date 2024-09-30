using System;

namespace TodoList.Api.Exceptions
{
    public class DbUpdateConcurrencyIdNotFoundException : Exception
    {
        public DbUpdateConcurrencyIdNotFoundException() { }

        public DbUpdateConcurrencyIdNotFoundException(string message)
            : base(message) { }

        public DbUpdateConcurrencyIdNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}

using System;

namespace TodoList.Api.Exceptions
{
    public class NewTodoItemDescriptionExistException : Exception
    {
        public NewTodoItemDescriptionExistException() { }

        public NewTodoItemDescriptionExistException(string message)
            : base(message) { }
    }
}

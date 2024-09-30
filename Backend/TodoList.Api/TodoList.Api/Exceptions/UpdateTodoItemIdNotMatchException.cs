using System;

namespace TodoList.Api.Exceptions
{
    public class UpdateTodoItemIdNotMatchException : Exception
    {
        public UpdateTodoItemIdNotMatchException() { }

        public UpdateTodoItemIdNotMatchException(string message) : base(message) { }
    }
}

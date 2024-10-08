using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TodoList.Api.Models;
using TodoList.Api.Repositories;
using TodoList.Api.Exceptions;

namespace TodoList.Api.Services
{
    public interface ITodoItemsService
    {
        Task<List<TodoItem>> GetTodoItemsAsync();

        Task<TodoItem> GetTodoItemAsync(Guid id);

        Task UpdateTodoItemAsync(Guid id, TodoItem item);

        Task AddTodoItemAsync(TodoItem item);
    }

    public class TodoItemsService : ITodoItemsService
    {
        private readonly ITodoItemsRepository _repository;

        public TodoItemsService(ITodoItemsRepository repository)
        {
            _repository = repository;
        }

        public async Task AddTodoItemAsync(TodoItem item)
        {
            if (string.IsNullOrEmpty(item?.Description))
            {
                throw new NewTodoItemMissDescriptionException();
            }
            else if (_repository.TodoItemDescriptionExists(item.Description))
            {
                throw new NewTodoItemDescriptionExistException();
            }
            if (item.Id == Guid.Empty) item.Id = Guid.NewGuid();
            await _repository.AddTodoItemAsync(item);
        }

        public async Task<TodoItem> GetTodoItemAsync(Guid id)
        {
            return await _repository.GetTodoItemAsync(id);
        }

        public async Task<List<TodoItem>> GetTodoItemsAsync()
        {
            return await _repository.GetTodoItemsAsync();
        }

        public async Task UpdateTodoItemAsync(Guid id, TodoItem item)
        {
            if (id != item.Id)
            {
                throw new UpdateTodoItemIdNotMatchException($"The given Id: {id}; The given item id: {item.Id}");
            }
            await _repository.UpdateTodoItemAsync(item);
        }
    }
}

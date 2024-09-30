using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Exceptions;
using TodoList.Api.Models;

namespace TodoList.Api.Repositories
{
    public interface ITodoItemsRepository
    {
        Task<List<TodoItem>> GetTodoItemsAsync();

        Task<TodoItem> GetTodoItemAsync(Guid id);

        Task UpdateTodoItemAsync(TodoItem item);

        Task AddTodoItemAsync(TodoItem item);

        bool TodoItemDescriptionExists(string description);
    }

    public class TodoItemsRepository : ITodoItemsRepository
    {
        private readonly TodoContext _context;        

        public TodoItemsRepository(TodoContext context)
        {
            _context = context;            
        }

        public async Task AddTodoItemAsync(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task<TodoItem> GetTodoItemAsync(Guid id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<List<TodoItem>> GetTodoItemsAsync()
        {
            return await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
        }

        public async Task UpdateTodoItemAsync(TodoItem item)
        {
            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TodoItemIdExists(item.Id))
                {
                    throw new DbUpdateConcurrencyIdNotFoundException(ex.Message, ex.InnerException);
                }
                
                throw ex;            
            }
        }

        private bool TodoItemIdExists(Guid id)
        {
            return _context.TodoItems.Any(x => x.Id == id);
        }
         

        public bool TodoItemDescriptionExists(string description)
        {
            return _context.TodoItems
                   .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TodoList.Api.Exceptions;
using TodoList.Api.Models;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemsService _service;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(ITodoItemsService service, ILogger<TodoItemsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItemsAsync()
        {
            var results = await _service.GetTodoItemsAsync();
            return Ok(results);
        }

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        [ActionName(nameof(GetTodoItemAsync))]
        public async Task<IActionResult> GetTodoItemAsync(Guid id)
        {
            var result = await _service.GetTodoItemAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/TodoItems/... 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItemAsync(Guid id, TodoItem todoItem)
        {
            try
            {
                await _service.UpdateTodoItemAsync(id, todoItem);
            }
            catch (UpdateTodoItemIdNotMatchException uex)
            {
                _logger.LogError(uex, uex.Message);
                return BadRequest("The given Id doesn't match to the id of the given todoItem");
            }            
            catch (DbUpdateConcurrencyIdNotFoundException updIdEx)
            {
                _logger.LogError(updIdEx, updIdEx.Message);
                return Problem(statusCode: StatusCodes.Status500InternalServerError);
            }            
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(statusCode: StatusCodes.Status500InternalServerError);
            }
                        
            return NoContent();
        } 

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostTodoItemAsync(TodoItem todoItem)
        {
            try
            {
                await _service.AddTodoItemAsync(todoItem);
            }
            catch (NewTodoItemMissDescriptionException mex)
            {
                _logger.LogError(mex, mex.Message);
                return BadRequest("Description is required");
            }
            catch (NewTodoItemDescriptionExistException eex)
            {
                _logger.LogError(eex, eex.Message);
                return BadRequest("Description already exists");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(statusCode: StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction(nameof(GetTodoItemAsync), new { id = todoItem.Id }, todoItem);            
        }         
    }
}

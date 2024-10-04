using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Api.Controllers;
using TodoList.Api.Exceptions;
using TodoList.Api.Models;
using TodoList.Api.Repositories;
using TodoList.Api.Services;
using Xunit;

namespace TodoList.Api.UnitTests.Controllers
{
    public class TodoItemsControllerTest
    {
        [Fact]
        public async Task PostTodoItemAsync_MissDescription_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new TodoItem { Id = id, Description = "Test" };
            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(m => m.AddTodoItemAsync(It.IsAny<TodoItem>())).ThrowsAsync(new NewTodoItemMissDescriptionException());
            
            var mocklogger = new Mock<ILogger<TodoItemsController>>();             
            
            var controller = new TodoItemsController(mockService.Object, mocklogger.Object);

            // Act
            var result = await controller.PostTodoItemAsync(item);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PostTodoItemAsync_DescriptionExist_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new TodoItem { Id = id, Description = "Test" };
            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(m => m.AddTodoItemAsync(It.IsAny<TodoItem>())).ThrowsAsync(new NewTodoItemDescriptionExistException());

            var mocklogger = new Mock<ILogger<TodoItemsController>>();

            var controller = new TodoItemsController(mockService.Object, mocklogger.Object);

            // Act
            var result = await controller.PostTodoItemAsync(item);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PostTodoItemAsync_GeneralError_ReturnsServerError()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new TodoItem { Id = id, Description = "Test" };
            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(m => m.AddTodoItemAsync(It.IsAny<TodoItem>())).ThrowsAsync(new Exception());

            var mocklogger = new Mock<ILogger<TodoItemsController>>();

            var controller = new TodoItemsController(mockService.Object, mocklogger.Object);

            // Act
            var result = await controller.PostTodoItemAsync(item);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public async Task PostTodoItemAsync_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new TodoItem { Id = id, Description = "Test", IsCompleted = false };
            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(m => m.AddTodoItemAsync(It.IsAny<TodoItem>()));

            var mocklogger = new Mock<ILogger<TodoItemsController>>();

            var controller = new TodoItemsController(mockService.Object, mocklogger.Object);

            // Act
            var result = await controller.PostTodoItemAsync(item);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(TodoItemsController.GetTodoItemAsync), createdAtActionResult.ActionName);
            Assert.Equal(id, createdAtActionResult.RouteValues["id"]);

            Assert.Equal(StatusCodes.Status201Created, createdAtActionResult?.StatusCode);
            var returnValue = Assert.IsType<TodoItem>(createdAtActionResult.Value);
            Assert.Equal(id, returnValue.Id);
            Assert.Equal("Test", returnValue.Description);
            Assert.False(returnValue.IsCompleted);
        }

        [Fact]
        public async Task PutTodoItemAsync_IdNotMatch_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new TodoItem { Id = id, Description = "Test" };
            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(m => m.UpdateTodoItemAsync(id, It.IsAny<TodoItem>())).ThrowsAsync(new UpdateTodoItemIdNotMatchException());

            var mocklogger = new Mock<ILogger<TodoItemsController>>();

            var controller = new TodoItemsController(mockService.Object, mocklogger.Object);

            // Act
            var result = await controller.PutTodoItemAsync(id, item);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutTodoItemAsync_ConcurrencyIdNotFound_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new TodoItem { Id = id, Description = "Test" };
            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(m => m.UpdateTodoItemAsync(id, It.IsAny<TodoItem>())).ThrowsAsync(new DbUpdateConcurrencyIdNotFoundException());

            var mocklogger = new Mock<ILogger<TodoItemsController>>();

            var controller = new TodoItemsController(mockService.Object, mocklogger.Object);

            // Act
            var result = await controller.PutTodoItemAsync(id, item);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public async Task PutTodoItemAsync_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new TodoItem { Id = id, Description = "Test" };
            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(m => m.UpdateTodoItemAsync(id, It.IsAny<TodoItem>()));

            var mocklogger = new Mock<ILogger<TodoItemsController>>();

            var controller = new TodoItemsController(mockService.Object, mocklogger.Object);

            // Act
            var result = await controller.PutTodoItemAsync(id, item);

            // Assert
            var objectResult = Assert.IsType<NoContentResult>(result);            
        }

        [Fact]
        public async Task GetTodoItemAsync_NotFound_ReturnNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new TodoItem { Id = id, Description = "Test" };
            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(m => m.GetTodoItemAsync(id)).ReturnsAsync((TodoItem)null);

            var mocklogger = new Mock<ILogger<TodoItemsController>>();

            var controller = new TodoItemsController(mockService.Object, mocklogger.Object);

            // Act
            var result = await controller.GetTodoItemAsync(id);

            // Assert
            var objectResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}

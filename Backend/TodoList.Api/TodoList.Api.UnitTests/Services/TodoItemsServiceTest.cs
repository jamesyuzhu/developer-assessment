using Moq;
using System;
using System.Threading.Tasks;
using TodoList.Api.Exceptions;
using TodoList.Api.Models;
using TodoList.Api.Repositories;
using TodoList.Api.Services;
using Xunit;

namespace TodoList.Api.UnitTests.Services
{
    public class TodoItemsServiceTest
    {
        [Fact]
        public async Task AddTodoItemAsync_NoDescription_ThrowsNoDescriptionException()
        {
            // Arrange
            var mockRepo = new Mock<ITodoItemsRepository>();             

            var service = new TodoItemsService(mockRepo.Object);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<NewTodoItemMissDescriptionException>(() => service.AddTodoItemAsync(new Models.TodoItem { }));
        }

        [Fact]
        public async Task AddTodoItemAsync_DescriptionExist_ThrowsDescriptionExistException()
        {
            // Arrange
            var mockRepo = new Mock<ITodoItemsRepository>();
            mockRepo.Setup(m => m.TodoItemDescriptionExists(It.IsAny<string>())).Returns(true);

            var service = new TodoItemsService(mockRepo.Object);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<NewTodoItemDescriptionExistException>(() => service.AddTodoItemAsync(new Models.TodoItem { Id = new Guid(), Description = "Test" }));
        }

        [Fact]
        public async Task AddTodoItemAsync_Success()
        {
            // Arrange
            var mockRepo = new Mock<ITodoItemsRepository>();
            mockRepo.Setup(m => m.TodoItemDescriptionExists(It.IsAny<string>())).Returns(false);
            mockRepo.Setup(m => m.AddTodoItemAsync(It.IsAny<Models.TodoItem>()));

            var service = new TodoItemsService(mockRepo.Object);

            // Act and result
           await service.AddTodoItemAsync(new Models.TodoItem { Id = new Guid(), Description = "Test" });
        }

        [Fact]
        public async Task UpdateTodoItemAsync_IdNotMatch_ThrowsNotMatchException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new TodoItem { Id = Guid.NewGuid(), Description = "Test" };
            var mockRepo = new Mock<ITodoItemsRepository>();
            
            var service = new TodoItemsService(mockRepo.Object);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<UpdateTodoItemIdNotMatchException>(() => service.UpdateTodoItemAsync(id, item));
        }

        [Fact]
        public async Task UpdateTodoItemAsync_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new TodoItem { Id = id, Description = "Test" };
            var mockRepo = new Mock<ITodoItemsRepository>();
            mockRepo.Setup(m => m.UpdateTodoItemAsync(It.IsAny<Models.TodoItem>()));
            
            var service = new TodoItemsService(mockRepo.Object);

            // Act and Assert
            await service.UpdateTodoItemAsync(id, item);
        }
    }
}

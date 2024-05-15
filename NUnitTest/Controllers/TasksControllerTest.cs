using Database.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Interface;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace Tests.Controllers
{
    [TestFixture]
    public class TasksControllerTests
    {
        private Mock<ITaskService> _mockTaskService;
        private TasksController _tasksController;

        [SetUp]
        public void Setup()
        {
            _mockTaskService = new Mock<ITaskService>();
            _tasksController = new TasksController(_mockTaskService.Object);
        }

        [Test]
        public async Task Get_ReturnsOkResultWithTasksList()
        {
            // Arrange
            var tasksList = new List<Tasks> {new Tasks { Id = 1,
                CreateDate = DateTime.Now,
                Deadline = DateTime.Now,
                Description = "srs",
                Name = "Get-Task",
                Status = Database.@enum.Status.InProgress,
                UpdateDate = null
            },
            new Tasks { Id = 2,
                CreateDate = DateTime.Now,
                Deadline = DateTime.Now,
                Description = "srs",
                Name = "Get-Task",
                Status = Database.@enum.Status.InProgress,
                UpdateDate = null
            }};
            _mockTaskService.Setup(service => service.GetTask()).ReturnsAsync(tasksList);

            // Act
            var result = await _tasksController.Get();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo(tasksList));
        }

        [Test]
        public async Task Get_WithId_ReturnsOkResultWithTask()
        {
            // Arrange
            int taskId = 1;
            var task = new Tasks { Id = taskId,
                CreateDate = DateTime.Now,
                Deadline = DateTime.Now,
                Description = "Description",
                Name = "Get-Task",
                Status = Database.@enum.Status.InProgress,
                UpdateDate = null
            };
            _mockTaskService.Setup(service => service.GetTask(taskId)).ReturnsAsync(task);

            // Act
            var result = await _tasksController.Get(taskId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo(task));
        }

        [Test]
        public async Task Post_WithValidTask_ReturnsCreatedAtAction()
        {
            // Arrange
            var validTask = new Tasks {
                CreateDate = DateTime.Now,
                Deadline = DateTime.Now,
                Description = "Description",
                Name = "Post-Task",
                Status = Database.@enum.Status.ToDo,
                UpdateDate = null
            };
            _mockTaskService.Setup(service => service.Post(validTask)).ReturnsAsync(validTask);

            // Act
            var result = await _tasksController.Post(validTask);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdAtActionResult = (CreatedAtActionResult)result;
            Assert.That(createdAtActionResult.ActionName, Is.EqualTo(nameof(TasksController.Post)));
            Assert.That(createdAtActionResult.Value, Is.EqualTo(validTask));
        }

        [Test]
        public async Task Post_WithNullTask_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = await _tasksController.Post(null);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Post_WithInvalidModel_ReturnsBadRequestWithModelState()
        {
            // Arrange
            _tasksController.ModelState.AddModelError("Name", "Name field required");
            int StatusCode = 400;

            // Act
            var result = await _tasksController.Post(new Tasks());

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObjectResult = (BadRequestObjectResult)result;
            Assert.That(badRequestObjectResult.StatusCode, Is.EqualTo(StatusCode));
        }

        [Test]
        public async Task Delete_WithValidId_ReturnsOk()
        {
            // Arrange
            int taskId = 1;
            _mockTaskService.Setup(service => service.Delete(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _tasksController.Delete(taskId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo("Task Deleted"));
        }

        [Test]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int taskId = 1;
            _mockTaskService.Setup(service => service.Delete(It.IsAny<int>()));

            // Act
            var result = await _tasksController.Delete(taskId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Put_WithMatchingId_ReturnsOk()
        {
            // Arrange
            int taskId = 1;
            var task = new Tasks {
                Id = 1,
                CreateDate = DateTime.Now,
                Deadline = DateTime.Now,
                Description = "Description",
                Name = "Update-Task",
                Status = Database.@enum.Status.ToDo,
                UpdateDate = null
            };
            _mockTaskService.Setup(service => service.Update(task)).ReturnsAsync(task);

            // Act
            var result = await _tasksController.Put(taskId, task);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo("Task Updated!"));
        }

        [Test]
        public async Task Put_WithMismatchingId_ReturnsBadRequest()
        {
            // Arrange
            int taskId = 1;
            var task = new Tasks { 
                Id = taskId + 1,
                CreateDate = DateTime.Now,
                Deadline = DateTime.Now,
                Description = "Description",
                Name = "update-Task",
                Status = Database.@enum.Status.ToDo,
                UpdateDate = null
            };

            // Act
            var result = await _tasksController.Put(taskId, task);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Put_WithNullResult_ReturnsNoContent()
        {
            // Arrange
            int taskId = 1;
            var task = new Tasks { 
                Id = 1,
                CreateDate = DateTime.Now,
                Deadline = DateTime.Now,
                Description = "Description",
                Name = "Post-Task",
                Status = Database.@enum.Status.ToDo,
                UpdateDate = null
            };
            _mockTaskService.Setup(service => service.Update(It.IsAny<Tasks>())).ReturnsAsync((Tasks)null);

            // Act
            var result = await _tasksController.Put(taskId, task);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}

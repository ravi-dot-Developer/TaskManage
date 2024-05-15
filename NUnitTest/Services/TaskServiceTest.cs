using Database;
using Database.Model;
using Microsoft.EntityFrameworkCore;
using Services;

namespace NUnitTest.Services
{
    [TestFixture]
    public class TaskServiceTest
    {

        [Test]
        public async Task GetTask_ReturnsTasksList()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var taskService = new TaskService(dbContext);

            //Act
            var result = await taskService.GetTask();

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<Tasks>>(result);
        }

        [Test]
        public async Task Delete_TaskExists_ReturnsTrue()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var taskService = new TaskService(dbContext);
            // Act
            var result = await taskService.Delete(1);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Delete_TaskDoesNotExist_ReturnsNull()
        {
            // Arrange
            int taskId = 100;
            var dbContext = await GetDatabaseContext();
            var taskService = new TaskService(dbContext);

            // Act
            var result = await taskService.Delete(taskId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetTask_Returns_Task_IfExists()
        {
            // Arrange
            Tasks expectedTask = new Tasks() { Id = 5 };
            int idToFind = 5;

            var dbContext = await GetDatabaseContext();
            var taskService = new TaskService(dbContext);
            
            // Act
            var result = await taskService.GetTask(idToFind);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(expectedTask.Id));
        }

        [Test]
        public async Task GetTask_Returns_Null_IfNotFound()
        {
            // Arrange
            int idToFind = 100;

            var dbContext = await GetDatabaseContext();
            var taskService = new TaskService(dbContext);

            // Act
            var result = await taskService.GetTask(idToFind);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task Post_Task_Success()
        {
            // Arrange
            Tasks expectedTask = new Tasks()
            {
                CreateDate = DateTime.Now,
                Deadline = DateTime.Now,
                Description = "srs",
                Name = "Post-Task",
                Status = Database.@enum.Status.ToDo,
                UpdateDate = null
            };
            var dbContext = await GetDatabaseContext();
            var taskService = new TaskService(dbContext);

            // Act
            var result = await taskService.Post(expectedTask);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(expectedTask.Name));
            Assert.That(result.Status, Is.EqualTo(expectedTask.Status));

        }

        [Test]
        public async Task Update_Task_When_NotExist_Return_Null()
        {
            // Arrange
            Tasks expectedTask = new Tasks()
            {   Id=100,
                CreateDate = DateTime.Now,
                Deadline = DateTime.Now,
                Description = "srs",
                Name = "Post-Task",
                Status = Database.@enum.Status.ToDo,
                UpdateDate = null
            };
            var dbContext = await GetDatabaseContext();
            var taskService = new TaskService(dbContext);

            // Act
            var result = await taskService.Update(expectedTask);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task Update_Task_Success()
        {
            // Arrange
            Tasks expectedTask = new Tasks()
            {
                Id = 9,
                CreateDate = DateTime.Now,
                Deadline = DateTime.Now,
                Description = "srs",
                Name = "Update-Task",
                Status = Database.@enum.Status.InProgress,
                UpdateDate = null
            };

            var dbContext = await GetDatabaseContext();
            var taskService = new TaskService(dbContext);

            // Detach any existing entity with the same key
            var existingTask = dbContext.Tasks.Find(expectedTask.Id);
            if (existingTask != null)
            {
                dbContext.Entry(existingTask).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }

            // Act
            var result = await taskService.Update(expectedTask);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(expectedTask.Name));
            Assert.That(result.Id, Is.EqualTo(expectedTask.Id));
            Assert.That(result.Status, Is.EqualTo(expectedTask.Status));
        }
        private async Task<TaskDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new TaskDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (databaseContext.Tasks.Count() <= 0)
            {
                for (int i = 1; i <= 10; i++)
                {
                  var newTask=
                    new Tasks()
                    {
                        CreateDate = DateTime.Now,
                        Deadline = DateTime.Now,
                        Description = "Description 1",
                        Id = i,
                        Name = "stt",
                        Status = Database.@enum.Status.ToDo,
                        UpdateDate = DateTime.Now
                    };
                   // databaseContext.Entry(newTask).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                   // var existingTask = databaseContext.Tasks.Find(newTask.Id);
                    //if (existingTask ==null)
                    //{
                        databaseContext.Tasks.Add(newTask);
                        await databaseContext.SaveChangesAsync();
                    //}
                   
                }

            }
            return databaseContext;
        }
    }
}



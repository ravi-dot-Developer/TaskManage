using Database;
using Database.Model;
using Microsoft.EntityFrameworkCore;
using Services.Interface;
using System.Net.Http.Headers;

namespace Services
{
    public class TaskService : ITaskService
    {
        private TaskDbContext dbContext;
        public TaskService(TaskDbContext taskDbContext)
        {
            dbContext = taskDbContext;
        }

        public async Task<bool?> Delete(int id)
        {
            var task = await dbContext.Tasks.FindAsync(id);
            if (task == null)
                return null;

            dbContext.Tasks.Remove(task);
            await dbContext.SaveChangesAsync();

            return  true;
        }

        public async Task<List<Tasks>> GetTask()
        {
            return await dbContext.Tasks.ToListAsync();
        }

        public async Task<Tasks?> GetTask(int id)
        {
            var result = await dbContext.Tasks.SingleOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<Tasks> Post(Tasks task)
        {
            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();
            return task;
        }

        public async Task<Tasks> Update(Tasks tasks)
        {
            if (!TasksExists(tasks.Id))
                return null;

            tasks.UpdateDate = DateTime.Now;
            dbContext.Update(tasks);

            await dbContext.SaveChangesAsync();
           

            return tasks;
        }
        private bool TasksExists(long id) => dbContext.Tasks.Any(x => x.Id == id);

    }
}

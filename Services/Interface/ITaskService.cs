using Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ITaskService
    {
        public Task<List<Tasks>> GetTask();
        public Task<Tasks?> GetTask(int id);
        public Task<Tasks> Post(Tasks task);
        public Task<Tasks> Update(Tasks tasks);
        public Task<bool?> Delete(int id);
    }
}

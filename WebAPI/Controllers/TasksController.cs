using Database.Model;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Threading.Tasks;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result= await _taskService.GetTask();
            if (result != null)
                return Ok(result);

            return NoContent();
        }

    
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _taskService.GetTask(id);
            if(result!=null)
              return Ok(result);

            return NoContent();
        }

        [HttpPost]    
        public async Task<IActionResult> Post(Tasks tasks)
        {
            if(tasks==null)
                return BadRequest();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _taskService.Post(tasks);

            return CreatedAtAction(nameof(Post), new { id = tasks.Id }, tasks);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Tasks tasks)
        {
            if (id != tasks.Id)
                return BadRequest();

            var result = await _taskService.Update(tasks);

            if (result != null)
                return Ok("Task Updated!");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result=await _taskService.Delete(id);
            if (result == null)
                return NotFound();

            return Ok("Task Deleted");
        }
    }
}

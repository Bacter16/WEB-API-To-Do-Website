using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Route("api/todos")]
    [ApiController]
    [Authorize]
    public class TodoListController : ControllerBase
    {
        private readonly ToDoListDBContext _database;

        public TodoListController(ToDoListDBContext context)
        {
            _database = context;
        }

        // GET: api/todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoTask>>> GetTodos()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var id = userIdClaim.Value;

            if (_database.Users.FirstOrDefault(u => u.UserId.ToString() == id) == null)
            {
                return NotFound();
            }

            var userTasks = await _database.TodoTasks
                .Where(task => task.UserId.ToString() == id && task.IsCompleted == false)
                .ToListAsync();

            return userTasks;
        }

        // POST: api/todos
        [HttpPost]
        public ActionResult<TodoTask> AddTodo([FromBody] TaskModel task)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!;
            var id = userIdClaim.Value;
            var user = _database.Users.FirstOrDefault(u => u.UserId.ToString() == id);

            if (user == null)
            {
                return Unauthorized();
            }

            var todo = new TodoTask
            {
                Title = task.Title,
                Description = task.Description,
                UserId = user.UserId,
            };

            _database.TodoTasks.Add(todo);
            _database.SaveChanges();
            return Ok(todo);
        }

        // PUT: api/todos/1
        [HttpPut("{id}")]
        public ActionResult<TodoTask> CompleteTask(Guid id)
        {
            var todoToUpdate = _database.TodoTasks.FirstOrDefault(t => t.Id == id);

            if (todoToUpdate != null)
            {
                todoToUpdate.IsCompleted = true;
                todoToUpdate.CompletedAt = DateTime.Now;
                _database.TodoTasks.Update(todoToUpdate);
                _database.SaveChanges();
                return DeleteTask(id);
            }

            return NotFound();
        }

        // DELETE: api/todos/1
        [HttpDelete("{id}")]
        public ActionResult<TodoTask> DeleteTask(Guid id)
        {
            var todoToDelete = _database.TodoTasks.FirstOrDefault(t => t.Id == id);

            if (todoToDelete != null)
            {
                _database.TodoTasks.Remove(todoToDelete);
                _database.SaveChanges();
                return Ok(todoToDelete);
            }

            return NotFound();
        }

    }
}

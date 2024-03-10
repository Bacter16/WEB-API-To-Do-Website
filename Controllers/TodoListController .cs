using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TodoTask>>> GetTodos(Guid id)
        {

            var userTasks = await _database.TodoTasks
                .Include(task => task.User)
                .Where(task => task.UserId == id)
                .ToListAsync();

            return userTasks;
        }

        // POST: api/todos
        [HttpPost]
        public ActionResult<TodoTask> AddTodo([FromBody] TaskModel task)
        {
            var username = HttpContext.User.Identity!.Name;

            var user = _database.Users.FirstOrDefault(u => u.Username == username);

            var todo = new TodoTask
            {
                Title = task.Title,
                Description = task.Description,
                UserId = user.UserId
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
                return Ok(todoToUpdate);
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

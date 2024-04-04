using ToDoList.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Data
{
    public class ToDoListDBContext : DbContext
    {
        public ToDoListDBContext(DbContextOptions<ToDoListDBContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<TodoTask> TodoTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        
        }

    }
}

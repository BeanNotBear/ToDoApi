using Microsoft.EntityFrameworkCore;

namespace ToDoApi
{
	public class ToDoDb : DbContext
	{
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public ToDoDb(DbContextOptions<ToDoDb> options) : base(options)
		{
		}
	}
}

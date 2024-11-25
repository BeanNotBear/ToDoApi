using Microsoft.EntityFrameworkCore;
using ToDoApi;

var builder = WebApplication.CreateBuilder(args);

// Add DI - AddService
builder.Services.AddDbContext<ToDoDb>(opt => opt.UseInMemoryDatabase("ToDoList"));

var app = builder.Build();

// Configure pipeline - UseMethod 
app.MapGet("/todoitems", async (ToDoDb db) => await db.ToDoItems.ToListAsync());
app.MapGet("/todoitems/{id}", async (int id, ToDoDb db) => await db.ToDoItems.FindAsync(id));
app.MapPost("/todoitems", async (ToDoItem todo, ToDoDb db) =>
{
	db.ToDoItems.Add(todo);
	await db.SaveChangesAsync();
	return Results.Created($"/todoitems/{todo.Id}", todo);
});
app.MapPut("/todoitems/{id}", async (int id, ToDoItem inputTodo, ToDoDb db) =>
{
	var todo = await db.ToDoItems.FindAsync(id);
	if (todo == null)
	{
		return Results.NotFound();
	}
	todo.Name = inputTodo.Name;
	todo.IsComplete = inputTodo.IsComplete;
	await db.SaveChangesAsync();
	return Results.NoContent();
});
app.MapDelete("/todoitems/{id}", async (int id, ToDoDb db) =>
{
	var todo = await db.ToDoItems.FindAsync(id);
	if (todo == null)
	{
		return Results.NotFound();
	}
	db.ToDoItems.Remove(todo);
	await db.SaveChangesAsync();
	return Results.NoContent();
});

app.Run();

using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;

namespace TodoAPI
{
    public static class TodoEndpoints
    {
        public static void AddTodoEndpoints(this WebApplication webApp)
        {
            webApp.MapGet("/todoitems", async (TodoDB db) => {
                return await db.Todos.ToListAsync();
            });

            webApp.MapGet("/todoitems/complete", async (TodoDB db) => {
                return await db.Todos.Where(todo => todo.IsComplete).ToListAsync();
            });

            webApp.MapGet("/todoitems/{id}", async (int id, TodoDB db) => {
                return await db.Todos.FindAsync(id) is Todo todo ? Results.Ok(todo) : Results.NotFound();
            });

            webApp.MapPost("/todoitems", async (Todo todo, TodoDB db) => {
                var todoToCreate = db.Todos.Add(todo).Entity;
                await db.SaveChangesAsync();

                return Results.Created($"/todoitems/{todoToCreate.Id}", todoToCreate);
            });

            webApp.MapPut("todoitems/{id}", async (int id, Todo todo, TodoDB db) => {
                var existingTodo = await db.Todos.FindAsync(id);

                if (existingTodo is null)
                {
                    return Results.NotFound();
                }
                else
                {
                    existingTodo.Name = todo.Name;
                    existingTodo.IsComplete = todo.IsComplete;

                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }
            });

            webApp.MapDelete("/todoitems/{id}", async (int id, TodoDB db) => {
                var existingTodo = await db.Todos.FindAsync(id);

                if (existingTodo is null)
                {
                    return Results.NotFound();
                }
                else
                {
                    db.Todos.Remove(existingTodo);
                    await db.SaveChangesAsync();

                    return Results.Ok();
                }
            });
        }
    }
}
using TodoApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ToDoDB"];

builder.Services.AddDbContext<ToDoDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.UseHttpsRedirection();
app.UseCors();

app.MapGet("/", (ToDoDbContext db) => "Server is running!");

app.MapGet("/tasks", async (ToDoDbContext db) =>
{
   return await db.Items.ToListAsync();
});


app.MapPost("/tasks", async (ToDoDbContext db, Item newItem) =>
{
    db.Items.Add(newItem);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{newItem.Id}", newItem);
});

app.MapPut("/tasks/{id}", async ( int id, Item updateItem,ToDoDbContext db) =>
{
    var task = await db.Items.FindAsync(id);
    if (task == null)
        return Results.NotFound();
    task.Name = updateItem.Name;
    task.IsComplete = updateItem.IsComplete;
    await db.SaveChangesAsync();
    Console.WriteLine("task updated successfully");
    return Results.Ok(task);
});

app.MapDelete("/tasks/{id}", async (int id,ToDoDbContext db) =>
{
    var task = await db.Items.FindAsync(id);
    if (task == null)
        return Results.NotFound();
    db.Items.Remove(task);
    await db.SaveChangesAsync();
    return Results.Ok("task deleted successfully");
});
app.Run();

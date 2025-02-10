using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<ToDoDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("ToDoDB");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.MapGet("/", () => "Hello World!");

app.MapGet("/tasks/{id}", async (ToDoDbContext db, int id) =>
{
    var task = await db.Items.FindAsync(id);
    if (task == null)
        return Results.NotFound();
    return Results.Ok(task);
});

app.MapGet("/tasks", async (ToDoDbContext db) =>
{
    var tasks = await db.Items.ToListAsync();
    return Results.Ok(tasks);
});


app.MapPost("/tasks", async (ToDoDbContext db, Item newItem) =>
{
    db.Items.Add(newItem);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{newItem.Id}", newItem);
});

app.MapPut("/tasks/{id}", async (ToDoDbContext db, int id, Item updateItem) =>
{
    var task = await db.Items.FindAsync(id);
    if (task == null)
        return Results.NotFound();
    task.Name = updateItem.Name;
    task.IsComplete = updateItem.IsComplete;
    await db.SaveChangesAsync();
    return Results.Ok(task);
});

app.MapDelete("/tasks/{id}", async (ToDoDbContext db, int id) =>
{
    var task = await db.Items.FindAsync(id);
    if (task == null)
        return Results.NotFound();
    db.Items.Remove(task);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.Run();

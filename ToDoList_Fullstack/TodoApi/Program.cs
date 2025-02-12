using TodoApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ToDoDB"];

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
    new MySqlServerVersion(new Version(8, 0, 41))));

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; 
});

app.UseCors();

app.MapGet("/", (ToDoDbContext db) => "Server is running!");

app.MapGet("/tasks", async (ToDoDbContext db) =>
{
   return await db.Items.ToListAsync();
});

app.MapPost("/tasks", async (Item item, ToDoDbContext db) =>
{
    await db.Items.AddAsync(item);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{item.Id}", item);
});

app.MapPut("/tasks/{id}", async ( int id, bool IsComplete,ToDoDbContext db) =>
{
    var task = await db.Items.FindAsync(id);
    if (task == null)
        return Results.NotFound();
    task.IsComplete = IsComplete;
    await db.SaveChangesAsync();
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

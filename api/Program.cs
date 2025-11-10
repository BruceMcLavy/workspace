using HQV.Data;
using HQV.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("Default")
           ?? builder.Configuration["ConnectionStrings:Default"]
           ?? "Host=db;Port=5432;Database=app;Username=app;Password=app";

builder.Services.AddDbContext<AppDb>(opt =>
    opt.UseNpgsql(conn));

var cors = "allowDev";
builder.Services.AddCors(o => o.AddPolicy(cors, p =>
    p.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(cors);
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDb>();
    db.Database.Migrate();
}

app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/notes", async (AppDb db) =>
{
    Console.WriteLine("Retrieving Hot Reload Notes");
    return await db.Notes.OrderBy(n => n.Id).ToListAsync();
});

app.MapPost("/api/notes", async (AppDb db, Note note) =>
{
    Console.WriteLine("Creating A NEW Note");
    db.Notes.Add(note);
    await db.SaveChangesAsync();
    return Results.Created($"/api/notes/{note.Id}", note);
});

app.Run();

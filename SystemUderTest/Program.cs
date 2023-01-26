using Microsoft.EntityFrameworkCore;
using SystemUderTest;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ItemsDB>(opt => opt.UseInMemoryDatabase("List"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/items", async (ItemsDB db) =>
    await db.Items.ToListAsync());

app.MapGet("/items/IsPresent", async (ItemsDB db, bool filter ) =>
    await db.Items.Where(t => t.IsPresent == filter).ToListAsync());

app.MapGet("/items/{id}", async (Guid id, ItemsDB db) =>
    await db.Items.FindAsync(id)
        is Item item
            ? Results.Ok(item)
            : Results.NotFound("Not found"));

app.MapPost("/items", async (Item item, ItemsDB db) =>
{
    if (db.Items.Contains(item))
    {
        return Results.UnprocessableEntity("Item allready exists");
    }
    db.Items.Add(item);
    await db.SaveChangesAsync();

    return Results.Created($"/itemitems/{item.Id}", item);
});

app.MapPut("/items/{id}", async (Guid id, Item inputItem, ItemsDB db) =>
{
    var item = await db.Items.FindAsync(id);

    if (item is null) return Results.NotFound();

    item.Name = inputitem.Name;
    item.IsPresent = inputitem.IsPresent;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (Guid id, ItemsDB db) =>
{
    if (await db.Items.FindAsync(id) is Item item)
    {
        db.Items.Remove(item);
        await db.SaveChangesAsync();
        return Results.Ok(item);
    }

    return Results.NotFound();
});

app.Run();
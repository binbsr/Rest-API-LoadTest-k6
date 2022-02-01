using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CountryDb>(opt => opt.UseInMemoryDatabase("CountryList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.MapGet("/", () => "Hello World");

app.MapGet("/countries", async (CountryDb db) =>
    await db.Countries.ToListAsync());

app.MapGet("/countries/{id}", async (int id, CountryDb db) =>
    await db.Countries.FindAsync(id)
        is Country country
            ? Results.Ok(country)
            : Results.NotFound());

app.MapPost("/countries", async (Country country, CountryDb db) =>
{
    db.Countries.Add(country);
    await db.SaveChangesAsync();

    return Results.Created($"/countries/{country.Id}", country);
});

app.MapPut("/countries/{id}", async (int id, Country inputCountry, CountryDb db) =>
{
    var country = await db.Countries.FindAsync(id);

    if (country is null) return Results.NotFound();

    country.Name = inputCountry.Name;
    country.Population = inputCountry.Population;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/countries/{id}", async (int id, CountryDb db) =>
{
    if (await db.Countries.FindAsync(id) is Country country)
    {
        db.Countries.Remove(country);
        await db.SaveChangesAsync();
        return Results.Ok(country);
    }

    return Results.NotFound();
});

app.Run();

internal record Country(int Id)
{
    public string Name { get; set; } = default!;
    public int Population { get; set; } = default!;
}

class CountryDb : DbContext
{
    public CountryDb(DbContextOptions<CountryDb> options)
        : base(options) { }

    public DbSet<Country> Countries => Set<Country>();
}

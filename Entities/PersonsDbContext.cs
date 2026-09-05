using Microsoft.EntityFrameworkCore;

namespace Entities;

public class PersonsDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
         
        modelBuilder.Entity<Country>().ToTable("Countries");
        modelBuilder.Entity<Person>().ToTable("Persons");

        string countries_json = File.ReadAllText("countries.json");
        List<Country>? countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countries_json);
        string persons_json = File.ReadAllText("persons.json");
        List<Country>? persons = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(persons_json);

        modelBuilder.Entity<Country>().HasData(countries);
        modelBuilder.Entity<Person>().HasData(persons);
    }
}

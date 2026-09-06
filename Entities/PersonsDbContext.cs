using Microsoft.EntityFrameworkCore; 

namespace Entities;

public class PersonsDbContext : DbContext
{
    public PersonsDbContext(DbContextOptions options) : base(options)
    { 
    }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>().ToTable("Countries");
        modelBuilder.Entity<Person>().ToTable("Persons");
         
        string countriesJson = File.ReadAllText("countries.json");
        List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

        foreach (Country country in countries)
            modelBuilder.Entity<Country>().HasData(country);

         
        string personsJson =File.ReadAllText("persons.json");
        List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

        foreach (Person person in persons)
            modelBuilder.Entity<Person>().HasData(person);

        modelBuilder.Entity<Person>().Property(p => p.TIN)
            .HasColumnName("TaxIdentificationNumber")
            .HasColumnType("varchar(8)");

        modelBuilder.Entity<Person>().HasIndex(p => p.TIN).IsUnique();

        modelBuilder.Entity<Person>().ToTable(c => c.HasCheckConstraint("CK_Persons_TIN", "LEN([TaxIdentificationNumber]) = 8"));
    }
}

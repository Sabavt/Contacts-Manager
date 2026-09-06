using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

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


        string personsJson = File.ReadAllText("persons.json");
        List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

        foreach (Person person in persons)
            modelBuilder.Entity<Person>().HasData(person);
    }

    public List<Person> usp_GetAllPersons()
    {
        return Persons.FromSqlRaw("EXEC usp_GetAllPersons").ToList();
    }

    public int usp_InsertPerson(Person person)
    {
        SqlParameter[] sqlParameters = [new SqlParameter("@PersonID", person.PersonID),
         new SqlParameter("@PersonName", person.PersonName),
         new SqlParameter("@Email", person.Email),
         new SqlParameter("@DateOfBirth", person.DateOfBirth),
         new SqlParameter("@Gender", person.Gender),
         new SqlParameter("@Address", person.Address),
         new SqlParameter("@CountryID", person.CountryID),
         new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)];

        return Database.ExecuteSqlRaw(
            $"EXEC usp_InsertPerson @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @Address, @CountryID, @ReceiveNewsLetters",
            sqlParameters);
    }
}

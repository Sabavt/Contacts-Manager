using Microsoft.EntityFrameworkCore;

namespace Entities;

public class PersonsDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Country> Countries { get; set; }
}

namespace Entities
{
    /// <summary>
    /// Domain Model for Country
    /// </summary>
    public class Country
    {
        public Guid CountryID {  get; set; } = Guid.NewGuid();
        public string? CountryName { get; set; }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class usp_GetPersons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string usp_GetAllPersons = @"
                CREATE PROCEDURE usp_GetAllPersons
                AS
                BEGIN
                    SELECT * FROM Persons
                END";
            migrationBuilder.Sql(usp_GetAllPersons);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string usp_GetAllPersons = @"
                DROP PROCEDURE usp_GetAllPersons
                AS
                BEGIN
                    SELECT * FROM Persons
                END";
            migrationBuilder.Sql(usp_GetAllPersons);
        }
    }
}

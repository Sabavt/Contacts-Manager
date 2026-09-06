using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDelete_StoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string usp_UpdatePerson = @"CREATE PROCEDURE usp_UpdatePerson
                                @PersonID UNIQUEIDENTIFIER,
                                @PersonName NVARCHAR(40),
                                @Email NVARCHAR(40),
                                @DateOfBirth DATETIME2(7),
                                @Gender NVARCHAR(8), 
                                @CountryID UNIQUEIDENTIFIER,
                                @Address NVARCHAR(100), 
                                @ReceiveNewsLetters BIT
AS
                        Begin 
                            UPDATE Persons 
                            SET PersonName = @PersonName, Email = @Email, DateOfBirth = @DateOfBirth, Gender = @Gender, Address = @Address, CountryID = @CountryID, ReceiveNewsLetters = @ReceiveNewsLetters
                            WHERE PersonID = @PersonID
                        END";
            migrationBuilder.Sql(usp_UpdatePerson);
            string usp_DeletePerson = @"CREATE PROCEDURE usp_DeletePerson
                                @PersonID UNIQUEIDENTIFIER
                        AS
                        Begin 
                            DELETE FROM Persons 
                            WHERE PersonID = @PersonID
                        END";
            migrationBuilder.Sql(usp_DeletePerson); 
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DROP PROCEDURE usp_UpdatePerson 
                      DROP PROCEDURE usp_DeletePerson"
            );

        }
    }
}

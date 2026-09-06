using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            string usp_InsertPerson = @"ALTER OR CREATE PROCEDURE usp_InsertPerson
                                @PersonID UNIQUEIDENTIFIER,
                                @PersonName NVARCHAR(40),
                                @Email NVARCHAR(40),
                                @DateOfBirth DATETIME2(7),
                                @Gender NVARCHAR(8),
                                @Address NVARCHAR(100),
                                @CountryID UNIQUEIDENTIFIER,
                                @ReceiveNewsLetters BIT
                        AS
                        Begin 

                            INSERT INTO Persons (PersonID, PersonName, Email, DateOfBirth, Gender, Address, CountryID, ReceiveNewsLetters)
                            VALUES (@PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @Address, @CountryID, @ReceiveNewsLetters) 

                        END";

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Persons",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.Sql(usp_InsertPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {  
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.Sql(@"DROP PROCEDURE usp_InsertPerson");
        }
    }
}

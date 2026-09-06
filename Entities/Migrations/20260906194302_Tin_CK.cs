using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Tin_CK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Persons_TaxIdentificationNumber",
                table: "Persons",
                column: "TaxIdentificationNumber",
                unique: true,
                filter: "[TaxIdentificationNumber] IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Persons_TIN",
                table: "Persons",
                sql: "LEN([TaxIdentificationNumber]) = 8");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Persons_TaxIdentificationNumber",
                table: "Persons");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Persons_TIN",
                table: "Persons");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiveNewsLetters = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonID);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryID", "CountryName" },
                values: new object[,]
                {
                    { new Guid("268690d6-f1de-4cc8-bc53-5f3d254f7220"), "Norway" },
                    { new Guid("30118c4e-f405-41ed-9634-540ca02c446f"), "Spain" },
                    { new Guid("44e36eb2-c1d9-42a3-8676-451c04e47f79"), "Brazil" },
                    { new Guid("69201c23-589b-45f3-88a7-97ef4de0bc66"), "Georgia" },
                    { new Guid("8fbd2d4a-ef69-41ad-8d99-aa947eae5e85"), "Italy" },
                    { new Guid("b55de8fd-9c27-4860-8c43-443bbd0151d1"), "Japan" },
                    { new Guid("d801c7eb-bfa6-46b2-83dd-9a6afdb9028c"), "Australia" },
                    { new Guid("ec5fea9a-76d9-4ce6-afa3-8eac27c3caab"), "Germany" },
                    { new Guid("fa2d98af-fb39-4d22-ba2e-c4f01b3f50da"), "France" },
                    { new Guid("fc08ff13-3464-40ed-862f-d83a9a1063a4"), "Canada" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonID", "Address", "CountryID", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("0a00f0ca-eece-4783-810f-95de9a0e5fb8"), "939 Park Way", new Guid("ec5fea9a-76d9-4ce6-afa3-8eac27c3caab"), new DateTime(1990, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "eva.brooks53@example.com", "Female", "Eva", true },
                    { new Guid("0cb42fe1-316a-452c-9446-c4af47861a2d"), "16 Cedar Way", new Guid("b55de8fd-9c27-4860-8c43-443bbd0151d1"), new DateTime(2002, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "julia.wilson29@mailbox.org", "Female", "Julia", true },
                    { new Guid("0d7c2bd1-a943-4ccf-a112-23774d2a1621"), "420 Main Street", new Guid("268690d6-f1de-4cc8-bc53-5f3d254f7220"), new DateTime(1994, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "samuel.parker34@sample.net", "Female", "Samuel", true },
                    { new Guid("0e36435d-79aa-4d4b-8d88-9d060b28589c"), "509 Hill Drive", new Guid("30118c4e-f405-41ed-9634-540ca02c446f"), new DateTime(1981, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "maya.wilson5@mailbox.org", "Female", "Maya", false },
                    { new Guid("0f471c8f-e4fb-40ce-b9c2-d223d8f44b8b"), "319 Main Street", new Guid("268690d6-f1de-4cc8-bc53-5f3d254f7220"), new DateTime(1998, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "david.brooks7@proton.me", "Male", "David", true },
                    { new Guid("142fa1ca-7bf0-400b-b714-0d1b977308b1"), "605 Hill Avenue", new Guid("69201c23-589b-45f3-88a7-97ef4de0bc66"), new DateTime(1979, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "ethan.wilson1@example.com", "Male", "Ethan", false },
                    { new Guid("1a7be9e7-4325-447f-890c-01a137d2e14b"), "416 Park Road", new Guid("ec5fea9a-76d9-4ce6-afa3-8eac27c3caab"), new DateTime(1995, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "oliver.turner4@example.com", "Male", "Oliver", false },
                    { new Guid("1ab8adeb-84cb-4a8e-8514-76901c180f49"), "83 River Street", new Guid("fc08ff13-3464-40ed-862f-d83a9a1063a4"), new DateTime(1983, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "victor.foster47@proton.me", "Female", "Victor", true },
                    { new Guid("20f1f287-764a-48e1-ad67-99ce44e5c235"), "246 Main Avenue", new Guid("268690d6-f1de-4cc8-bc53-5f3d254f7220"), new DateTime(1988, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "sofia.morgan13@outlook.com", "Male", "Sofia", false },
                    { new Guid("21690135-0ca3-40aa-b5f9-48766c6b3299"), "127 Main Way", new Guid("d801c7eb-bfa6-46b2-83dd-9a6afdb9028c"), new DateTime(1979, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "eva.brooks2@example.com", "Female", "Eva", true },
                    { new Guid("2323792f-de16-42d0-a83e-701ef6ff956d"), "4 Hill Road", new Guid("268690d6-f1de-4cc8-bc53-5f3d254f7220"), new DateTime(1985, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "anna.miller39@example.com", "Male", "Anna", true },
                    { new Guid("244ed71b-1b2c-45c6-8522-7390fe2800f1"), "505 Main Street", new Guid("d801c7eb-bfa6-46b2-83dd-9a6afdb9028c"), new DateTime(1979, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "ethan.foster59@mailbox.org", "Female", "Ethan", false },
                    { new Guid("29cd8ebc-7af2-49c3-be75-5f1097b37299"), "138 Oak Circle", new Guid("b55de8fd-9c27-4860-8c43-443bbd0151d1"), new DateTime(1984, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "ethan.brooks56@sample.net", "Male", "Ethan", true },
                    { new Guid("30daf9c5-4cfa-4cd0-a3e6-10c7d1648a56"), "635 Cedar Road", new Guid("8fbd2d4a-ef69-41ad-8d99-aa947eae5e85"), new DateTime(2002, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "sofia.reed36@example.com", "Male", "Sofia", false },
                    { new Guid("359a593e-e027-43c7-a9da-512b4d384ed5"), "809 Main Street", new Guid("ec5fea9a-76d9-4ce6-afa3-8eac27c3caab"), new DateTime(1990, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "julia.parker6@example.com", "Female", "Julia", false },
                    { new Guid("35c319c9-04f1-4fc2-a5f6-118310eb0634"), "412 Main Avenue", new Guid("268690d6-f1de-4cc8-bc53-5f3d254f7220"), new DateTime(1981, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "noah.hayes9@example.com", "Female", "Noah", false },
                    { new Guid("36d8cbd1-36fa-467e-98a5-907fc21c2ef8"), "675 Main Street", new Guid("d801c7eb-bfa6-46b2-83dd-9a6afdb9028c"), new DateTime(1989, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "lucas.carter20@outlook.com", "Female", "Lucas", true },
                    { new Guid("39d44177-39f0-4f48-ab55-71375935295f"), "688 Garden Drive", new Guid("b55de8fd-9c27-4860-8c43-443bbd0151d1"), new DateTime(1978, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "noah.reed57@sample.net", "Female", "Noah", true },
                    { new Guid("3b5c4b23-de5a-4c58-a6e9-3318d8c85a6d"), "108 Lake Street", new Guid("30118c4e-f405-41ed-9634-540ca02c446f"), new DateTime(1980, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "arthur.cooper0@outlook.com", "Female", "Arthur", false },
                    { new Guid("3cda8351-2412-4b93-baad-cbcf1614d0be"), "159 Oak Avenue", new Guid("fa2d98af-fb39-4d22-ba2e-c4f01b3f50da"), new DateTime(1999, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "emma.hayes30@mailbox.org", "Male", "Emma", false },
                    { new Guid("4abed33c-243f-46f0-887b-06ef26bf4658"), "634 Hill Road", new Guid("69201c23-589b-45f3-88a7-97ef4de0bc66"), new DateTime(2000, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "victor.turner19@example.com", "Female", "Victor", false },
                    { new Guid("50269cc9-c416-4389-aaf6-2aee3b95266f"), "560 Park Road", new Guid("ec5fea9a-76d9-4ce6-afa3-8eac27c3caab"), new DateTime(1986, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "ethan.hayes52@mailbox.org", "Female", "Ethan", true },
                    { new Guid("526f39e6-e78a-4180-9920-ad0e17fdc7bd"), "40 Lake Avenue", new Guid("fa2d98af-fb39-4d22-ba2e-c4f01b3f50da"), new DateTime(1988, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "henry.parker43@mailbox.org", "Male", "Henry", false },
                    { new Guid("540e46cd-5333-4e6f-96e6-ff9165d878ac"), "718 Park Way", new Guid("69201c23-589b-45f3-88a7-97ef4de0bc66"), new DateTime(1994, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "oliver.carter17@proton.me", "Male", "Oliver", true },
                    { new Guid("5546019b-56f1-4062-86de-efb608eb2867"), "37 Park Street", new Guid("268690d6-f1de-4cc8-bc53-5f3d254f7220"), new DateTime(1983, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "mila.carter50@outlook.com", "Male", "Mila", true },
                    { new Guid("6323b06e-1bf6-47ba-87dd-1c8f981ef966"), "163 Hill Avenue", new Guid("69201c23-589b-45f3-88a7-97ef4de0bc66"), new DateTime(1990, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "mila.turner48@sample.net", "Male", "Mila", false },
                    { new Guid("6879d142-2c5e-4498-9059-1f130d799cf3"), "586 Cedar Way", new Guid("44e36eb2-c1d9-42a3-8676-451c04e47f79"), new DateTime(1990, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "julia.hayes45@example.com", "Female", "Julia", true },
                    { new Guid("6c26f12f-93a6-4112-9ee5-dd491a0ebf03"), "504 Hill Circle", new Guid("8fbd2d4a-ef69-41ad-8d99-aa947eae5e85"), new DateTime(1989, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "emma.morgan46@sample.net", "Female", "Emma", false },
                    { new Guid("7574b888-f5c2-4de5-8018-25ef8a08f81a"), "307 Cedar Street", new Guid("fa2d98af-fb39-4d22-ba2e-c4f01b3f50da"), new DateTime(1982, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "alex.reed15@sample.net", "Male", "Alex", false },
                    { new Guid("796b64c0-0246-4258-b6d7-b02b69dbf3e9"), "14 Main Way", new Guid("268690d6-f1de-4cc8-bc53-5f3d254f7220"), new DateTime(1993, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "lucas.foster14@proton.me", "Female", "Lucas", true },
                    { new Guid("7f0f5968-038d-40d5-87d2-aabc964a5070"), "449 Oak Road", new Guid("d801c7eb-bfa6-46b2-83dd-9a6afdb9028c"), new DateTime(2000, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "julia.carter32@sample.net", "Female", "Julia", false },
                    { new Guid("8546322a-964c-4dc1-a148-95df9a6eb551"), "707 Lake Circle", new Guid("8fbd2d4a-ef69-41ad-8d99-aa947eae5e85"), new DateTime(1998, 7, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "arthur.bennett22@proton.me", "Male", "Arthur", false },
                    { new Guid("85dfdbd8-5655-4a9e-870d-e23a3ca3bbf2"), "59 Hill Way", new Guid("30118c4e-f405-41ed-9634-540ca02c446f"), new DateTime(1999, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "henry.brooks33@mailbox.org", "Male", "Henry", false },
                    { new Guid("8b96c45e-5414-4f61-85c3-805948f5da2c"), "588 Cedar Circle", new Guid("fc08ff13-3464-40ed-862f-d83a9a1063a4"), new DateTime(1981, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "clara.turner21@example.com", "Female", "Clara", false },
                    { new Guid("8f4dfb56-b095-4a26-b80c-2b3b19f664f2"), "571 Oak Way", new Guid("d801c7eb-bfa6-46b2-83dd-9a6afdb9028c"), new DateTime(1999, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "mila.morgan44@sample.net", "Male", "Mila", true },
                    { new Guid("90152fe6-dd08-46d2-a97a-7e6c786df209"), "618 Park Road", new Guid("44e36eb2-c1d9-42a3-8676-451c04e47f79"), new DateTime(1985, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "sofia.carter40@proton.me", "Male", "Sofia", false },
                    { new Guid("934a1f5e-d7cf-4a01-8835-00d067ef9eea"), "261 River Circle", new Guid("8fbd2d4a-ef69-41ad-8d99-aa947eae5e85"), new DateTime(1987, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "noah.reed27@mailbox.org", "Male", "Noah", true },
                    { new Guid("94d3006d-1235-4dba-9002-be4b54560083"), "290 River Street", new Guid("69201c23-589b-45f3-88a7-97ef4de0bc66"), new DateTime(1980, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "elena.bennett8@example.com", "Male", "Elena", false },
                    { new Guid("9b4804ef-4cf5-4c72-9394-fe0a86dd6deb"), "766 River Circle", new Guid("30118c4e-f405-41ed-9634-540ca02c446f"), new DateTime(1979, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "mila.morgan54@outlook.com", "Male", "Mila", false },
                    { new Guid("9ba3d887-f6f7-40a7-8019-4f2ff1d2f492"), "407 Cedar Street", new Guid("b55de8fd-9c27-4860-8c43-443bbd0151d1"), new DateTime(1988, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "daniel.cooper10@mailbox.org", "Male", "Daniel", true },
                    { new Guid("9bf95620-6f77-4e45-bd85-24d037eb7ede"), "449 Oak Road", new Guid("8fbd2d4a-ef69-41ad-8d99-aa947eae5e85"), new DateTime(1988, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "victor.foster26@outlook.com", "Female", "Victor", false },
                    { new Guid("9fd96a82-3de6-42ac-8170-e8391aa30bd8"), "687 Garden Avenue", new Guid("44e36eb2-c1d9-42a3-8676-451c04e47f79"), new DateTime(1983, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "maya.parker38@example.com", "Female", "Maya", true },
                    { new Guid("a0be6b91-f5d1-4a8b-8885-43f33158e2ef"), "466 Lake Avenue", new Guid("fa2d98af-fb39-4d22-ba2e-c4f01b3f50da"), new DateTime(1997, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "daniel.miller25@mailbox.org", "Female", "Daniel", true },
                    { new Guid("a3f8cdec-52ca-4897-82c4-643266c1aeeb"), "110 Cedar Circle", new Guid("30118c4e-f405-41ed-9634-540ca02c446f"), new DateTime(1981, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "nora.wilson35@outlook.com", "Male", "Nora", true },
                    { new Guid("aad09c55-d059-43ec-9b22-cff2fbbcd82d"), "353 Cedar Circle", new Guid("d801c7eb-bfa6-46b2-83dd-9a6afdb9028c"), new DateTime(1983, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "lucas.parker58@proton.me", "Female", "Lucas", true },
                    { new Guid("b6c80a40-c651-4375-8628-0c819c93dfc0"), "771 Garden Street", new Guid("30118c4e-f405-41ed-9634-540ca02c446f"), new DateTime(1979, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "mila.cooper51@sample.net", "Male", "Mila", false },
                    { new Guid("b6fe77ea-96a6-42b9-a4de-f6a0759ae7a6"), "779 River Street", new Guid("30118c4e-f405-41ed-9634-540ca02c446f"), new DateTime(1999, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "arthur.turner28@sample.net", "Female", "Arthur", false },
                    { new Guid("bc924452-ca1a-44ac-adce-38db5d2c3f2a"), "943 Cedar Circle", new Guid("fa2d98af-fb39-4d22-ba2e-c4f01b3f50da"), new DateTime(2001, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "nora.morgan11@sample.net", "Female", "Nora", true },
                    { new Guid("bcd560a3-3b3d-42d1-b337-4260939cc415"), "661 River Circle", new Guid("44e36eb2-c1d9-42a3-8676-451c04e47f79"), new DateTime(1998, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "lucas.parker49@example.com", "Female", "Lucas", true },
                    { new Guid("be35387e-e876-4821-a823-72701af61406"), "685 Main Drive", new Guid("69201c23-589b-45f3-88a7-97ef4de0bc66"), new DateTime(1981, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "alex.reed12@sample.net", "Male", "Alex", false },
                    { new Guid("c6f05580-8ba0-4379-8f0d-8d963f988ca2"), "250 Cedar Road", new Guid("fa2d98af-fb39-4d22-ba2e-c4f01b3f50da"), new DateTime(1981, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "henry.morgan55@proton.me", "Male", "Henry", true },
                    { new Guid("d2576ac4-1fbd-4e87-8bb3-077d90e9417f"), "526 Lake Way", new Guid("ec5fea9a-76d9-4ce6-afa3-8eac27c3caab"), new DateTime(1979, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "arthur.carter3@example.com", "Male", "Arthur", true },
                    { new Guid("d35d084b-555e-4e2d-a145-d84e02312d8a"), "988 Oak Drive", new Guid("44e36eb2-c1d9-42a3-8676-451c04e47f79"), new DateTime(2001, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "emma.brooks37@sample.net", "Male", "Emma", false },
                    { new Guid("d983f139-62b2-4af5-b74b-576a18fb6d77"), "883 River Drive", new Guid("b55de8fd-9c27-4860-8c43-443bbd0151d1"), new DateTime(1983, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "lucas.hayes31@outlook.com", "Male", "Lucas", false },
                    { new Guid("de8308b7-a16c-4960-afe2-be2d67b58974"), "891 River Road", new Guid("fc08ff13-3464-40ed-862f-d83a9a1063a4"), new DateTime(1995, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "henry.brooks18@sample.net", "Male", "Henry", false },
                    { new Guid("e0762d0f-099d-4cad-8914-0a772e170fc4"), "93 River Street", new Guid("8fbd2d4a-ef69-41ad-8d99-aa947eae5e85"), new DateTime(1988, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "maya.foster16@proton.me", "Female", "Maya", true },
                    { new Guid("e27b9e04-b564-4dd7-9af5-76389744d5b0"), "866 Hill Street", new Guid("d801c7eb-bfa6-46b2-83dd-9a6afdb9028c"), new DateTime(1988, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "sofia.miller41@outlook.com", "Male", "Sofia", false },
                    { new Guid("fb90e3f9-99b8-4bc1-be75-8a23cfbc70c9"), "215 Hill Avenue", new Guid("fa2d98af-fb39-4d22-ba2e-c4f01b3f50da"), new DateTime(2001, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "daniel.morgan24@proton.me", "Male", "Daniel", true },
                    { new Guid("fcba0b12-3ed3-4b2f-a02b-a7dfb2e465c1"), "988 River Road", new Guid("268690d6-f1de-4cc8-bc53-5f3d254f7220"), new DateTime(1986, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "mila.morgan42@mailbox.org", "Female", "Mila", true },
                    { new Guid("ff13a68c-4fd0-41b3-8fd2-15a05303cdfb"), "682 Lake Street", new Guid("d801c7eb-bfa6-46b2-83dd-9a6afdb9028c"), new DateTime(1991, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "david.bennett23@proton.me", "Female", "David", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APDB_Kolokwium_template.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Book_ID = table.Column<int>(type: "int", nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => new { x.Book_ID, x.ID });
                    table.ForeignKey(
                        name: "FK_Loans_Books_Book_ID",
                        column: x => x.Book_ID,
                        principalTable: "Books",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_Users_ID",
                        column: x => x.ID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBookFavorites",
                columns: table => new
                {
                    Book_ID = table.Column<int>(type: "int", nullable: false),
                    User_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBookFavorites", x => new { x.Book_ID, x.User_ID });
                    table.ForeignKey(
                        name: "FK_UserBookFavorites_Books_Book_ID",
                        column: x => x.Book_ID,
                        principalTable: "Books",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBookFavorites_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "ID", "Author", "Title" },
                values: new object[,]
                {
                    { 101, "Andrzej Sapkowski", "Wiedźmin" },
                    { 102, "Stanisław Lem", "Solaris" },
                    { 103, "Adam Mickiewicz", "Pan Tadeusz" },
                    { 104, "Bolesław Prus", "Lalka" },
                    { 105, "Antoine de Saint-Exupéry", "Mały Książę" },
                    { 201, "J.K. Rowling", "Harry Potter i Kamień Filozoficzny" },
                    { 205, "J.R.R. Tolkien", "Władca Pierścieni" },
                    { 210, "George Orwell", "1984" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "anna.nowak@example.com", "Anna Nowak" },
                    { 2, "jan.kowalski@example.com", "Jan Kowalski" },
                    { 3, "maria.wisniewska@example.com", "Maria Wiśniewska" }
                });

            migrationBuilder.InsertData(
                table: "Loans",
                columns: new[] { "Book_ID", "ID", "LoanDate", "ReturnDate" },
                values: new object[,]
                {
                    { 101, 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 102, 2, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 105, 1, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "UserBookFavorites",
                columns: new[] { "Book_ID", "User_ID" },
                values: new object[,]
                {
                    { 101, 1 },
                    { 102, 2 },
                    { 103, 2 },
                    { 105, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loans_ID",
                table: "Loans",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookFavorites_User_ID",
                table: "UserBookFavorites",
                column: "User_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "UserBookFavorites");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

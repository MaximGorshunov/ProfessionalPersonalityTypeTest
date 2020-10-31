using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DBRepository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Professions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    ProfType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAdmin = table.Column<bool>(nullable: false),
                    Login = table.Column<string>(maxLength: 20, nullable: false),
                    Email = table.Column<string>(maxLength: 30, nullable: false),
                    Birthdate = table.Column<DateTime>(nullable: false),
                    IsMan = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfessionIdFirst = table.Column<int>(nullable: false),
                    ProfessionIdSecond = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Professions_ProfessionIdFirst",
                        column: x => x.ProfessionIdFirst,
                        principalTable: "Professions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_Professions_ProfessionIdSecond",
                        column: x => x.ProfessionIdSecond,
                        principalTable: "Professions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    R = table.Column<int>(nullable: false),
                    I = table.Column<int>(nullable: false),
                    A = table.Column<int>(nullable: false),
                    S = table.Column<int>(nullable: false),
                    E = table.Column<int>(nullable: false),
                    C = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserResults_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ProfessionIdFirst",
                table: "Questions",
                column: "ProfessionIdFirst");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ProfessionIdSecond",
                table: "Questions",
                column: "ProfessionIdSecond");

            migrationBuilder.CreateIndex(
                name: "IX_UserResults_UserId",
                table: "UserResults",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "UserResults");

            migrationBuilder.DropTable(
                name: "Professions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

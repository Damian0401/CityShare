using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityShare.Backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EmailTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailPriorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RetryNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailPriorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    Receiver = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TryCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Status = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailPrirorityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emails_EmailPriorities_EmailPrirorityId",
                        column: x => x.EmailPrirorityId,
                        principalTable: "EmailPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emails_EmailPrirorityId",
                table: "Emails",
                column: "EmailPrirorityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "EmailPriorities");
        }
    }
}

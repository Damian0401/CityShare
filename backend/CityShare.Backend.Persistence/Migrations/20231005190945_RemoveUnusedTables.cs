using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityShare.Backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emails_EmailPriorities_PrirorityId",
                table: "Emails");

            migrationBuilder.DropTable(
                name: "EmailPriorities");

            migrationBuilder.DropIndex(
                name: "IX_Emails_PrirorityId",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "PrirorityId",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "TryCount",
                table: "Emails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrirorityId",
                table: "Emails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TryCount",
                table: "Emails",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_Emails_PrirorityId",
                table: "Emails",
                column: "PrirorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_EmailPriorities_PrirorityId",
                table: "Emails",
                column: "PrirorityId",
                principalTable: "EmailPriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityShare.Backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EmailStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emails_EmailPriorities_EmailPrirorityId",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Emails");

            migrationBuilder.RenameColumn(
                name: "EmailPrirorityId",
                table: "Emails",
                newName: "StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Emails_EmailPrirorityId",
                table: "Emails",
                newName: "IX_Emails_StatusId");

            migrationBuilder.AddColumn<int>(
                name: "PrirorityId",
                table: "Emails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EmailStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailStatuses", x => x.Id);
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

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_EmailStatuses_StatusId",
                table: "Emails",
                column: "StatusId",
                principalTable: "EmailStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emails_EmailPriorities_PrirorityId",
                table: "Emails");

            migrationBuilder.DropForeignKey(
                name: "FK_Emails_EmailStatuses_StatusId",
                table: "Emails");

            migrationBuilder.DropTable(
                name: "EmailStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Emails_PrirorityId",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "PrirorityId",
                table: "Emails");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Emails",
                newName: "EmailPrirorityId");

            migrationBuilder.RenameIndex(
                name: "IX_Emails_StatusId",
                table: "Emails",
                newName: "IX_Emails_EmailPrirorityId");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Emails",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_EmailPriorities_EmailPrirorityId",
                table: "Emails",
                column: "EmailPrirorityId",
                principalTable: "EmailPriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

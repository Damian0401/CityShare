using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityShare.Backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CorrectTypos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SendDate",
                table: "Emails",
                newName: "SentDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SentDate",
                table: "Emails",
                newName: "SendDate");
        }
    }
}

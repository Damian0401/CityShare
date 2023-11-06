using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityShare.Backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Events_EventId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_EventId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Requests");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ImageId",
                table: "Requests",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Images_ImageId",
                table: "Requests",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Images_ImageId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ImageId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Requests");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Requests_EventId",
                table: "Requests",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Events_EventId",
                table: "Requests",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}

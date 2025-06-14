using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutFut.Notify.Service.Migrations
{
    /// <inheritdoc />
    public partial class initii : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OS",
                table: "Devices");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Devices",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Devices");

            migrationBuilder.AddColumn<int>(
                name: "OS",
                table: "Devices",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

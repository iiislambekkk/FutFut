using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutFut.Profile.Service.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayedHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Device = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayedHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemWorks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    TimeOfWork = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemWorks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DescriptionPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false),
                    ProfileEntityId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DescriptionPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DescriptionPhotos_Profiles_ProfileEntityId",
                        column: x => x.ProfileEntityId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DescriptionPhotos_ProfileEntityId",
                table: "DescriptionPhotos",
                column: "ProfileEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DescriptionPhotos");

            migrationBuilder.DropTable(
                name: "PlayedHistory");

            migrationBuilder.DropTable(
                name: "SystemWorks");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}

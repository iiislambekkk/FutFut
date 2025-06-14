using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutFut.Profile.Service.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DescriptionPhotos");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Profiles",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "Profiles",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Profiles",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileEntityId",
                table: "PlayedHistory",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "PlayedHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AboutPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false),
                    ProfileEntityId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutPhotos_Profiles_ProfileEntityId",
                        column: x => x.ProfileEntityId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayedHistory_ProfileEntityId",
                table: "PlayedHistory",
                column: "ProfileEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AboutPhotos_ProfileEntityId",
                table: "AboutPhotos",
                column: "ProfileEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayedHistory_Profiles_ProfileEntityId",
                table: "PlayedHistory",
                column: "ProfileEntityId",
                principalTable: "Profiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayedHistory_Profiles_ProfileEntityId",
                table: "PlayedHistory");

            migrationBuilder.DropTable(
                name: "AboutPhotos");

            migrationBuilder.DropIndex(
                name: "IX_PlayedHistory_ProfileEntityId",
                table: "PlayedHistory");

            migrationBuilder.DropColumn(
                name: "About",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ProfileEntityId",
                table: "PlayedHistory");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "PlayedHistory");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Profiles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "Profiles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

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
    }
}

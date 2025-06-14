using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutFut.Profile.Service.Migrations
{
    /// <inheritdoc />
    public partial class maybeFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowFriends",
                table: "Profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowPlayingSong",
                table: "Profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowRecentlyPlayed",
                table: "Profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileEntityId1",
                table: "PlayedHistory",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileEntityId1",
                table: "AboutPhotos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "AboutPhotos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "FriendShipEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RespondedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ProfileEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProfileEntityId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendShipEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendShipEntities_Profiles_ProfileEntityId",
                        column: x => x.ProfileEntityId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FriendShipEntities_Profiles_ProfileEntityId1",
                        column: x => x.ProfileEntityId1,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayedHistory_ProfileEntityId1",
                table: "PlayedHistory",
                column: "ProfileEntityId1");

            migrationBuilder.CreateIndex(
                name: "IX_AboutPhotos_ProfileEntityId1",
                table: "AboutPhotos",
                column: "ProfileEntityId1");

            migrationBuilder.CreateIndex(
                name: "IX_FriendShipEntities_ProfileEntityId",
                table: "FriendShipEntities",
                column: "ProfileEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendShipEntities_ProfileEntityId1",
                table: "FriendShipEntities",
                column: "ProfileEntityId1");

            migrationBuilder.CreateIndex(
                name: "IX_FriendShipEntities_RequestedUserId_RespondedUserId",
                table: "FriendShipEntities",
                columns: new[] { "RequestedUserId", "RespondedUserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AboutPhotos_Profiles_ProfileEntityId1",
                table: "AboutPhotos",
                column: "ProfileEntityId1",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayedHistory_Profiles_ProfileEntityId1",
                table: "PlayedHistory",
                column: "ProfileEntityId1",
                principalTable: "Profiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AboutPhotos_Profiles_ProfileEntityId1",
                table: "AboutPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayedHistory_Profiles_ProfileEntityId1",
                table: "PlayedHistory");

            migrationBuilder.DropTable(
                name: "FriendShipEntities");

            migrationBuilder.DropIndex(
                name: "IX_PlayedHistory_ProfileEntityId1",
                table: "PlayedHistory");

            migrationBuilder.DropIndex(
                name: "IX_AboutPhotos_ProfileEntityId1",
                table: "AboutPhotos");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ShowFriends",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ShowPlayingSong",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ShowRecentlyPlayed",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ProfileEntityId1",
                table: "PlayedHistory");

            migrationBuilder.DropColumn(
                name: "ProfileEntityId1",
                table: "AboutPhotos");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "AboutPhotos");
        }
    }
}

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
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Avatar = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    About = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ShowFriends = table.Column<bool>(type: "boolean", nullable: false),
                    ShowPlayingSong = table.Column<bool>(type: "boolean", nullable: false),
                    ShowRecentlyPlayed = table.Column<bool>(type: "boolean", nullable: false),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "AboutPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_AboutPhotos_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendShipRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendShipRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendShipRequests_Profiles_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FriendShipRequests_Profiles_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FriendShips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FriendAId = table.Column<Guid>(type: "uuid", nullable: false),
                    FriendBId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendShips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendShips_Profiles_FriendAId",
                        column: x => x.FriendAId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FriendShips_Profiles_FriendBId",
                        column: x => x.FriendBId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayedHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Device = table.Column<string>(type: "text", nullable: false),
                    ProfileEntityId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayedHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayedHistory_Profiles_ProfileEntityId",
                        column: x => x.ProfileEntityId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayedHistory_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutPhotos_ProfileEntityId",
                table: "AboutPhotos",
                column: "ProfileEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AboutPhotos_ProfileId",
                table: "AboutPhotos",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendShipRequests_FromUserId",
                table: "FriendShipRequests",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendShipRequests_ToUserId",
                table: "FriendShipRequests",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendShips_FriendAId",
                table: "FriendShips",
                column: "FriendAId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendShips_FriendBId",
                table: "FriendShips",
                column: "FriendBId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayedHistory_ProfileEntityId",
                table: "PlayedHistory",
                column: "ProfileEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayedHistory_ProfileId",
                table: "PlayedHistory",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutPhotos");

            migrationBuilder.DropTable(
                name: "FriendShipRequests");

            migrationBuilder.DropTable(
                name: "FriendShips");

            migrationBuilder.DropTable(
                name: "PlayedHistory");

            migrationBuilder.DropTable(
                name: "SystemWorks");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}

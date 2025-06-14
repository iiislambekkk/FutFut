using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutFut.Profile.Service.Migrations
{
    /// <inheritdoc />
    public partial class notFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AboutPhotos_Profiles_ProfileEntityId1",
                table: "AboutPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendShipEntities_Profiles_ProfileEntityId1",
                table: "FriendShipEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayedHistory_Profiles_ProfileEntityId1",
                table: "PlayedHistory");

            migrationBuilder.DropIndex(
                name: "IX_PlayedHistory_ProfileEntityId1",
                table: "PlayedHistory");

            migrationBuilder.DropIndex(
                name: "IX_FriendShipEntities_ProfileEntityId1",
                table: "FriendShipEntities");

            migrationBuilder.DropIndex(
                name: "IX_AboutPhotos_ProfileEntityId1",
                table: "AboutPhotos");

            migrationBuilder.DropColumn(
                name: "ProfileEntityId1",
                table: "PlayedHistory");

            migrationBuilder.DropColumn(
                name: "ProfileEntityId1",
                table: "FriendShipEntities");

            migrationBuilder.DropColumn(
                name: "ProfileEntityId1",
                table: "AboutPhotos");

            migrationBuilder.CreateIndex(
                name: "IX_PlayedHistory_ProfileId",
                table: "PlayedHistory",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendShipEntities_RespondedUserId",
                table: "FriendShipEntities",
                column: "RespondedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AboutPhotos_ProfileId",
                table: "AboutPhotos",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AboutPhotos_Profiles_ProfileId",
                table: "AboutPhotos",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendShipEntities_Profiles_RequestedUserId",
                table: "FriendShipEntities",
                column: "RequestedUserId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendShipEntities_Profiles_RespondedUserId",
                table: "FriendShipEntities",
                column: "RespondedUserId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayedHistory_Profiles_ProfileId",
                table: "PlayedHistory",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AboutPhotos_Profiles_ProfileId",
                table: "AboutPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendShipEntities_Profiles_RequestedUserId",
                table: "FriendShipEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendShipEntities_Profiles_RespondedUserId",
                table: "FriendShipEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayedHistory_Profiles_ProfileId",
                table: "PlayedHistory");

            migrationBuilder.DropIndex(
                name: "IX_PlayedHistory_ProfileId",
                table: "PlayedHistory");

            migrationBuilder.DropIndex(
                name: "IX_FriendShipEntities_RespondedUserId",
                table: "FriendShipEntities");

            migrationBuilder.DropIndex(
                name: "IX_AboutPhotos_ProfileId",
                table: "AboutPhotos");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileEntityId1",
                table: "PlayedHistory",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileEntityId1",
                table: "FriendShipEntities",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileEntityId1",
                table: "AboutPhotos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayedHistory_ProfileEntityId1",
                table: "PlayedHistory",
                column: "ProfileEntityId1");

            migrationBuilder.CreateIndex(
                name: "IX_FriendShipEntities_ProfileEntityId1",
                table: "FriendShipEntities",
                column: "ProfileEntityId1");

            migrationBuilder.CreateIndex(
                name: "IX_AboutPhotos_ProfileEntityId1",
                table: "AboutPhotos",
                column: "ProfileEntityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AboutPhotos_Profiles_ProfileEntityId1",
                table: "AboutPhotos",
                column: "ProfileEntityId1",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendShipEntities_Profiles_ProfileEntityId1",
                table: "FriendShipEntities",
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
    }
}

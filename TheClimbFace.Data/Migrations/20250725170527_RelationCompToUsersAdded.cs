using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheClimbFace.Data.Migrations
{
    /// <inheritdoc />
    public partial class RelationCompToUsersAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClimbingCompetitions_AspNetUsers_ApplicationUserId",
                table: "ClimbingCompetitions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationUserId",
                table: "ClimbingCompetitions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClimbingCompetitions_AspNetUsers_ApplicationUserId",
                table: "ClimbingCompetitions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClimbingCompetitions_AspNetUsers_ApplicationUserId",
                table: "ClimbingCompetitions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationUserId",
                table: "ClimbingCompetitions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ClimbingCompetitions_AspNetUsers_ApplicationUserId",
                table: "ClimbingCompetitions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

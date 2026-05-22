using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thefieldofdreams.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUserSelfRefFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ReferrerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ReferrerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReferrerId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReferredByUserId",
                table: "Users",
                column: "ReferredByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ReferredByUserId",
                table: "Users",
                column: "ReferredByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ReferredByUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ReferredByUserId",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "ReferrerId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReferrerId",
                table: "Users",
                column: "ReferrerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ReferrerId",
                table: "Users",
                column: "ReferrerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

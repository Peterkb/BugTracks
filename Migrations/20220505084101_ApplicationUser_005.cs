using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTracksV3.Migrations
{
    public partial class ApplicationUser_005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarContentType",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "AvatarFileData",
                table: "AspNetUsers",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarFileName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarContentType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AvatarFileData",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AvatarFileName",
                table: "AspNetUsers");
        }
    }
}

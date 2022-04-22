using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicComponentsShop.Database.Migrations
{
    public partial class Table_Users_Remove_Column_Password2_Add_Columns_ResetPasswordToken_and_ResetPasswordTokenExpireAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password2",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordTokenExpireAt",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetPasswordTokenExpireAt",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Password2",
                table: "Users",
                type: "text",
                nullable: true);
        }
    }
}

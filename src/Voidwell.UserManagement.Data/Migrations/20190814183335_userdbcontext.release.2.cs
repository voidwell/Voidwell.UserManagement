using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Voidwell.UserManagement.Data.Migrations
{
    public partial class userdbcontextrelease2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityQuestions_User_UserId",
                table: "SecurityQuestions");

            migrationBuilder.DropTable(
                name: "Authentication");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new Guid("0f20e7dc-32ef-45e1-9995-eb67f21c026d"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("46ecfcac-8e43-43ac-b953-e6e81c81c308"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: new Guid("f7f5096a-880f-49b7-95b6-1135000166e1"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("6acc7d94-a933-47d7-9ec6-df13cfe9dee0"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new Guid("46ecfcac-8e43-43ac-b953-e6e81c81c308"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("0f20e7dc-32ef-45e1-9995-eb67f21c026d"));

            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: new Guid("6acc7d94-a933-47d7-9ec6-df13cfe9dee0"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("f7f5096a-880f-49b7-95b6-1135000166e1"));

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Banned = table.Column<DateTimeOffset>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authentication",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LastLoginDate = table.Column<DateTimeOffset>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: false),
                    PasswordSalt = table.Column<string>(nullable: false),
                    PasswordSetDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authentication", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Authentication_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    DisplayName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    TimeZone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Profile_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityQuestions_User_UserId",
                table: "SecurityQuestions",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

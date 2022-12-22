using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Data.Migrations
{
    public partial class add_migration_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StringValue_AspNetUsers_UserId",
                table: "StringValue");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDetail_AspNetUsers_UserId",
                table: "UserDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDetail",
                table: "UserDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StringValue",
                table: "StringValue");

            migrationBuilder.RenameTable(
                name: "UserDetail",
                newName: "UserDetails");

            migrationBuilder.RenameTable(
                name: "StringValue",
                newName: "StringValues");

            migrationBuilder.RenameIndex(
                name: "IX_UserDetail_UserId",
                table: "UserDetails",
                newName: "IX_UserDetails_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserDetail_FirstName_Surname",
                table: "UserDetails",
                newName: "IX_UserDetails_FirstName_Surname");

            migrationBuilder.RenameIndex(
                name: "IX_StringValue_Value",
                table: "StringValues",
                newName: "IX_StringValues_Value");

            migrationBuilder.RenameIndex(
                name: "IX_StringValue_UserId",
                table: "StringValues",
                newName: "IX_StringValues_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDetails",
                table: "UserDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StringValues",
                table: "StringValues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StringValues_AspNetUsers_UserId",
                table: "StringValues",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_AspNetUsers_UserId",
                table: "UserDetails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StringValues_AspNetUsers_UserId",
                table: "StringValues");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_AspNetUsers_UserId",
                table: "UserDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDetails",
                table: "UserDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StringValues",
                table: "StringValues");

            migrationBuilder.RenameTable(
                name: "UserDetails",
                newName: "UserDetail");

            migrationBuilder.RenameTable(
                name: "StringValues",
                newName: "StringValue");

            migrationBuilder.RenameIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetail",
                newName: "IX_UserDetail_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserDetails_FirstName_Surname",
                table: "UserDetail",
                newName: "IX_UserDetail_FirstName_Surname");

            migrationBuilder.RenameIndex(
                name: "IX_StringValues_Value",
                table: "StringValue",
                newName: "IX_StringValue_Value");

            migrationBuilder.RenameIndex(
                name: "IX_StringValues_UserId",
                table: "StringValue",
                newName: "IX_StringValue_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDetail",
                table: "UserDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StringValue",
                table: "StringValue",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StringValue_AspNetUsers_UserId",
                table: "StringValue",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetail_AspNetUsers_UserId",
                table: "UserDetail",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

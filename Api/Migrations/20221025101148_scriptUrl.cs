using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class scriptUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScriptUrl",
                table: "Posts",
                newName: "Script");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Groups",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Script",
                table: "Posts",
                newName: "ScriptUrl");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Groups",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}

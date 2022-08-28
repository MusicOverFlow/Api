using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class scriptInCommentaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScriptLanguage",
                table: "Commentaries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScriptUrl",
                table: "Commentaries",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScriptLanguage",
                table: "Commentaries");

            migrationBuilder.DropColumn(
                name: "ScriptUrl",
                table: "Commentaries");
        }
    }
}

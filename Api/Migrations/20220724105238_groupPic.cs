using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class groupPic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilPicUrl",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "PicUrl",
                table: "Groups",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PicUrl",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicUrl",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "PicUrl",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "ProfilPicUrl",
                table: "Accounts",
                type: "text",
                nullable: true);
        }
    }
}

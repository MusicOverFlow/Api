using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class propertiesNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentaries_Accounts_AccountId",
                table: "Commentaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Accounts_AccountId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Posts",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_AccountId",
                table: "Posts",
                newName: "IX_Posts_OwnerId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Commentaries",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Commentaries_AccountId",
                table: "Commentaries",
                newName: "IX_Commentaries_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaries_Accounts_OwnerId",
                table: "Commentaries",
                column: "OwnerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Accounts_OwnerId",
                table: "Posts",
                column: "OwnerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentaries_Accounts_OwnerId",
                table: "Commentaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Accounts_OwnerId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Posts",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_OwnerId",
                table: "Posts",
                newName: "IX_Posts_AccountId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Commentaries",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Commentaries_OwnerId",
                table: "Commentaries",
                newName: "IX_Commentaries_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaries_Accounts_AccountId",
                table: "Commentaries",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Accounts_AccountId",
                table: "Posts",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

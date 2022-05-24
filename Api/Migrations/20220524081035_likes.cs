using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class likes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Commentaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccountCommentary",
                columns: table => new
                {
                    LikedCommentariesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LikesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountCommentary", x => new { x.LikedCommentariesId, x.LikesId });
                    table.ForeignKey(
                        name: "FK_AccountCommentary_Accounts_LikesId",
                        column: x => x.LikesId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountCommentary_Commentaries_LikedCommentariesId",
                        column: x => x.LikedCommentariesId,
                        principalTable: "Commentaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountPost",
                columns: table => new
                {
                    LikedPostsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LikesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPost", x => new { x.LikedPostsId, x.LikesId });
                    table.ForeignKey(
                        name: "FK_AccountPost_Accounts_LikesId",
                        column: x => x.LikesId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountPost_Posts_LikedPostsId",
                        column: x => x.LikedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountCommentary_LikesId",
                table: "AccountCommentary",
                column: "LikesId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPost_LikesId",
                table: "AccountPost",
                column: "LikesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountCommentary");

            migrationBuilder.DropTable(
                name: "AccountPost");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Commentaries");
        }
    }
}

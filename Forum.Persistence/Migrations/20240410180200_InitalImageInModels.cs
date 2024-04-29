using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.Persistence.Migrations
{
    public partial class InitalImageInModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_AspNetUsers_UserId",
                table: "Article");

            migrationBuilder.DropForeignKey(
                name: "FK_Article_Image_ImageId",
                table: "Article");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Image_PictureId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Article_ArticleId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_UserId",
                table: "Comment");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PictureId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Article",
                table: "Article");

            migrationBuilder.DropIndex(
                name: "IX_Article_ImageId",
                table: "Article");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountBan",
                table: "AccountBan");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Article");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "Article",
                newName: "Articles");

            migrationBuilder.RenameTable(
                name: "AccountBan",
                newName: "Bans");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_UserId",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_ArticleId",
                table: "Comments",
                newName: "IX_Comments_ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_Article_UserId",
                table: "Articles",
                newName: "IX_Articles_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Articles",
                table: "Articles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bans",
                table: "Bans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_UserId",
                table: "Articles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Articles_ArticleId",
                table: "Comments",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_UserId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Articles_ArticleId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bans",
                table: "Bans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Articles",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Articles");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameTable(
                name: "Bans",
                newName: "AccountBan");

            migrationBuilder.RenameTable(
                name: "Articles",
                newName: "Article");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "Comment",
                newName: "IX_Comment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ArticleId",
                table: "Comment",
                newName: "IX_Comment_ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_UserId",
                table: "Article",
                newName: "IX_Article_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "PictureId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Article",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountBan",
                table: "AccountBan",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Article",
                table: "Article",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PictureId",
                table: "AspNetUsers",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Article_ImageId",
                table: "Article",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_AspNetUsers_UserId",
                table: "Article",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Image_ImageId",
                table: "Article",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Image_PictureId",
                table: "AspNetUsers",
                column: "PictureId",
                principalTable: "Image",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Article_ArticleId",
                table: "Comment",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_UserId",
                table: "Comment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

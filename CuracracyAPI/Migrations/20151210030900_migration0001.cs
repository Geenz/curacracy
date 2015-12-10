using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace CuracracyAPI.Migrations
{
    public partial class migration0001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthenticatedSession",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    expirationDate = table.Column<DateTime>(nullable: false),
                    sessionId = table.Column<string>(nullable: true),
                    userid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticatedSession", x => x.id);
                });
            migrationBuilder.CreateTable(
                name: "AuthenticatedUser",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    hashMethod = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    salt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticatedUser", x => x.id);
                });
            migrationBuilder.CreateTable(
                name: "Folder",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true),
                    parentId = table.Column<long>(nullable: false),
                    permissions = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.id);
                });
            migrationBuilder.CreateTable(
                name: "FolderEntry",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    folderId = table.Column<long>(nullable: false),
                    submissionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderEntry", x => x.id);
                });
            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    mediaType = table.Column<int>(nullable: false),
                    mediaUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.id);
                });
            migrationBuilder.CreateTable(
                name: "Submission",
                columns: table => new
                {
                    submissionId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    description = table.Column<string>(nullable: true),
                    mediaId = table.Column<long>(nullable: false),
                    threadId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submission", x => x.submissionId);
                });
            migrationBuilder.CreateTable(
                name: "SubmissionMeta",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    posted = table.Column<DateTime>(nullable: false),
                    rating = table.Column<int>(nullable: false),
                    thumbnailUrl = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true),
                    @type = table.Column<int>(name: "type", nullable: false),
                    userId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionMeta", x => x.id);
                });
            migrationBuilder.CreateTable(
                name: "ThreadComment",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    commentBody = table.Column<string>(nullable: true),
                    commentParentId = table.Column<long>(nullable: false),
                    commentSubject = table.Column<string>(nullable: true),
                    commentThreadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadComment", x => x.id);
                });
            migrationBuilder.CreateTable(
                name: "ThreadMeta",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    threadStatus = table.Column<short>(nullable: false),
                    threadTopic = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadMeta", x => x.id);
                });
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    description = table.Column<string>(nullable: true),
                    galleryFolderId = table.Column<long>(nullable: false),
                    shoutsThreadId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userId);
                });
            migrationBuilder.CreateTable(
                name: "UserMeta",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    birthDate = table.Column<DateTime>(nullable: false),
                    email = table.Column<string>(nullable: true),
                    rank = table.Column<int>(nullable: false),
                    registrationDate = table.Column<DateTime>(nullable: false),
                    userName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMeta", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("AuthenticatedSession");
            migrationBuilder.DropTable("AuthenticatedUser");
            migrationBuilder.DropTable("Folder");
            migrationBuilder.DropTable("FolderEntry");
            migrationBuilder.DropTable("Media");
            migrationBuilder.DropTable("Submission");
            migrationBuilder.DropTable("SubmissionMeta");
            migrationBuilder.DropTable("ThreadComment");
            migrationBuilder.DropTable("ThreadMeta");
            migrationBuilder.DropTable("User");
            migrationBuilder.DropTable("UserMeta");
        }
    }
}

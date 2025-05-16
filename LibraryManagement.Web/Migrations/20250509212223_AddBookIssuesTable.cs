using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddBookIssuesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssuedBooks");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.CreateTable(
                name: "BookIssues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookRefId = table.Column<int>(type: "INTEGER", nullable: false),
                    MemberRefId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateDue = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateReturned = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    BookId = table.Column<int>(type: "INTEGER", nullable: true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookIssues_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookIssues_Books_BookRefId",
                        column: x => x.BookRefId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookIssues_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookIssues_Members_MemberRefId",
                        column: x => x.MemberRefId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookIssues_BookId",
                table: "BookIssues",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookIssues_BookRefId",
                table: "BookIssues",
                column: "BookRefId");

            migrationBuilder.CreateIndex(
                name: "IX_BookIssues_MemberId",
                table: "BookIssues",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_BookIssues_MemberRefId",
                table: "BookIssues",
                column: "MemberRefId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookIssues");

            migrationBuilder.CreateTable(
                name: "IssuedBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    BookId1 = table.Column<int>(type: "INTEGER", nullable: true),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuedBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuedBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuedBooks_Books_BookId1",
                        column: x => x.BookId1,
                        principalTable: "Books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IssuedBooks_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    GeneratedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    GeneratedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssuedBooks_BookId",
                table: "IssuedBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuedBooks_BookId1",
                table: "IssuedBooks",
                column: "BookId1");

            migrationBuilder.CreateIndex(
                name: "IX_IssuedBooks_MemberId",
                table: "IssuedBooks",
                column: "MemberId");
        }
    }
}

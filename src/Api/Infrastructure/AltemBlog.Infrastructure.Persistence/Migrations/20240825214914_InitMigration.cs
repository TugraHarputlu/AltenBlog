using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltemBlog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "emailconfirmation",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldMailAdresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewMailAdresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emailconfirmation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "entreis",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entreis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_entreis_users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "entryComment",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entryComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_entryComment_entreis_EntryId",
                        column: x => x.EntryId,
                        principalSchema: "dbo",
                        principalTable: "entreis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_entryComment_users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "entryFavorities",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entryFavorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_entryFavorities_entreis_EntryId",
                        column: x => x.EntryId,
                        principalSchema: "dbo",
                        principalTable: "entreis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_entryFavorities_users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "entryVote",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoteType = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entryVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_entryVote_entreis_EntryId",
                        column: x => x.EntryId,
                        principalSchema: "dbo",
                        principalTable: "entreis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "entryCommentFavorite",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entryCommentFavorite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_entryCommentFavorite_entryComment_EntryCommentId",
                        column: x => x.EntryCommentId,
                        principalSchema: "dbo",
                        principalTable: "entryComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_entryCommentFavorite_users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "entryCommentVote",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoteType = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entryCommentVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_entryCommentVote_entryComment_EntryCommentId",
                        column: x => x.EntryCommentId,
                        principalSchema: "dbo",
                        principalTable: "entryComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_entreis_CreatedById",
                schema: "dbo",
                table: "entreis",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_entryComment_CreatedById",
                schema: "dbo",
                table: "entryComment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_entryComment_EntryId",
                schema: "dbo",
                table: "entryComment",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_entryCommentFavorite_CreatedById",
                schema: "dbo",
                table: "entryCommentFavorite",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_entryCommentFavorite_EntryCommentId",
                schema: "dbo",
                table: "entryCommentFavorite",
                column: "EntryCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_entryCommentVote_EntryCommentId",
                schema: "dbo",
                table: "entryCommentVote",
                column: "EntryCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_entryFavorities_CreatedById",
                schema: "dbo",
                table: "entryFavorities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_entryFavorities_EntryId",
                schema: "dbo",
                table: "entryFavorities",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_entryVote_EntryId",
                schema: "dbo",
                table: "entryVote",
                column: "EntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "emailconfirmation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "entryCommentFavorite",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "entryCommentVote",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "entryFavorities",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "entryVote",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "entryComment",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "entreis",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "users",
                schema: "dbo");
        }
    }
}

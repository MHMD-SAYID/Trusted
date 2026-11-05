using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trusted.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_mediators_list_to_Account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountMediators",
                columns: table => new
                {
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    MediatorId = table.Column<long>(type: "bigint", nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountMediators", x => new { x.AccountId, x.MediatorId });
                    table.ForeignKey(
                        name: "FK_AccountMediators_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountMediators_AspNetUsers_MediatorId",
                        column: x => x.MediatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountMediators_MediatorId",
                table: "AccountMediators",
                column: "MediatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountMediators");
        }
    }
}

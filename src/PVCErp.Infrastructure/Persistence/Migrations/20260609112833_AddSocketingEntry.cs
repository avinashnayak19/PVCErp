using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PVCErp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSocketingEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Socketings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PipesReceived = table.Column<int>(type: "int", nullable: false),
                    PipesSocketed = table.Column<int>(type: "int", nullable: false),
                    RejectedQty = table.Column<int>(type: "int", nullable: false),
                    Shift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScrapWeight = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socketings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Socketings");
        }
    }
}

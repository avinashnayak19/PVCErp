using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PVCErp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRawMaterialLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "RawMaterials",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "RawMaterials");
        }
    }
}

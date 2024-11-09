using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDimensionsColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Width",
                table: "Products",
                newName: "ProductDimensionsWidth");

            migrationBuilder.RenameColumn(
                name: "Length",
                table: "Products",
                newName: "ProductionDimensionsLength");

            migrationBuilder.RenameColumn(
                name: "Height",
                table: "Products",
                newName: "ProductDimensionsHeight");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductionDimensionsLength",
                table: "Products",
                newName: "Length");

            migrationBuilder.RenameColumn(
                name: "ProductDimensionsWidth",
                table: "Products",
                newName: "Width");

            migrationBuilder.RenameColumn(
                name: "ProductDimensionsHeight",
                table: "Products",
                newName: "Height");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RenameFieldNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "ProductCategory", "ProductName", "ProductPrice" },
                values: new object[,]
                {
                    { 1L, "DogFood", "Dry Dog Food", 50m },
                    { 2L, "DogFood", "Wet Dog Food", 35m },
                    { 3L, "DogFood", "Dog Treats", 10m },
                    { 4L, "Toys", "Chew Toy", 15m },
                    { 5L, "Toys", "Fetch Ball", 8m },
                    { 6L, "CollarsAndLeashes", "Dog Collar", 12m },
                    { 7L, "CollarsAndLeashes", "Dog Leash", 20m },
                    { 8L, "GroomingAndHygiene", "Dog Shampoo", 10m },
                    { 9L, "GroomingAndHygiene", "Dog Brush", 7m },
                    { 10L, "Beds", "Comfort Dog Bed", 80m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10L);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "ProductCategory", "ProductName", "ProductPrice" },
                values: new object[,]
                {
                    { 1, "DogFood", "Dry Dog Food", 50m },
                    { 2, "DogFood", "Wet Dog Food", 35m },
                    { 3, "DogFood", "Dog Treats", 10m },
                    { 4, "Toys", "Chew Toy", 15m },
                    { 5, "Toys", "Fetch Ball", 8m },
                    { 6, "CollarsAndLeashes", "Dog Collar", 12m },
                    { 7, "CollarsAndLeashes", "Dog Leash", 20m },
                    { 8, "GroomingAndHygiene", "Dog Shampoo", 10m },
                    { 9, "GroomingAndHygiene", "Dog Brush", 7m },
                    { 10, "Beds", "Comfort Dog Bed", 80m }
                });
        }
    }
}

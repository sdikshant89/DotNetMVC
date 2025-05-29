using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNet.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BrandAndRatingAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Products",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RatingNos",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "Brand", "Rating", "RatingNos" },
                values: new object[] { "Apple", 4.6m, 155 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "Brand", "Rating", "RatingNos" },
                values: new object[] { "ToyComp", 3.8m, 2000 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "Brand", "Rating", "RatingNos" },
                values: new object[] { "Samsung", 4.8m, 450 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Brand", "CategoryId", "Description", "ImageUrl", "Name", "Price", "Rating", "RatingNos" },
                values: new object[] { 4, "Logitech", 1, "Best DPI ranging from 2000 to 16000", null, "Gaming Wireless mouse", 120m, 4.8m, 4500 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RatingNos",
                table: "Products");
        }
    }
}

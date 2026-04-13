using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCartAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateCartHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "CartHeaders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "CartHeaders");
        }
    }
}

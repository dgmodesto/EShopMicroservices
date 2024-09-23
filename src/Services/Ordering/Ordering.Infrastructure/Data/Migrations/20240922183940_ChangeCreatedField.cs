using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCreatedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Products",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Orders",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "OrderItems",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Customers",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<int>(
                name: "Payment_PaymentMethod",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Products",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Orders",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "OrderItems",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Customers",
                newName: "Created");

            migrationBuilder.AlterColumn<string>(
                name: "Payment_PaymentMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

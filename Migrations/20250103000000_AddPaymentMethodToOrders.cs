using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaLaStore.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentMethodToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "CashOnDelivery");

            migrationBuilder.AddColumn<string>(
                name: "CardLast4",
                table: "Orders",
                type: "TEXT",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardHolderName",
                table: "Orders",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardExpiry",
                table: "Orders",
                type: "TEXT",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardExpiry",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CardHolderName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CardLast4",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Orders");
        }
    }
}










using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sc.Models.Migrations
{
    /// <inheritdoc />
    public partial class V102 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "receivedoffering",
                table: "receivedvoucherhistory",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "secondreceivedoffering",
                table: "receivedvoucherhistory",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "receivedoffering",
                table: "receivedvoucher",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "secondreceivedoffering",
                table: "receivedvoucher",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "receivedoffering",
                table: "receivedvoucherhistory");

            migrationBuilder.DropColumn(
                name: "secondreceivedoffering",
                table: "receivedvoucherhistory");

            migrationBuilder.DropColumn(
                name: "receivedoffering",
                table: "receivedvoucher");

            migrationBuilder.DropColumn(
                name: "secondreceivedoffering",
                table: "receivedvoucher");
        }
    }
}

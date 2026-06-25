using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Migrations
{
    /// <inheritdoc />
    public partial class columnrename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Bookings",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Bookings",
                newName: "ServiceDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceDate",
                table: "Bookings",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "Bookings",
                newName: "Name");
        }
    }
}

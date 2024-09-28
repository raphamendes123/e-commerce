using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Customer.API.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Zip",
                table: "Addresses",
                newName: "ZipCode");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Addresses",
                newName: "StreetAddress");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Addresses",
                newName: "BuildingNumber");

            migrationBuilder.RenameColumn(
                name: "Complement",
                table: "Addresses",
                newName: "SecondaryAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Addresses",
                newName: "Zip");

            migrationBuilder.RenameColumn(
                name: "StreetAddress",
                table: "Addresses",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "SecondaryAddress",
                table: "Addresses",
                newName: "Complement");

            migrationBuilder.RenameColumn(
                name: "BuildingNumber",
                table: "Addresses",
                newName: "Number");
        }
    }
}

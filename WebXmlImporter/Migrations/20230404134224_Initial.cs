using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebXmlImporter.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FullAddress",
                columns: table => new
                {
                    FullAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullAddress", x => x.FullAddressId);
                });

            migrationBuilder.CreateTable(
                name: "ShipInfo",
                columns: table => new
                {
                    ShipInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipVia = table.Column<int>(type: "int", nullable: false),
                    Freight = table.Column<double>(type: "float", nullable: false),
                    ShipName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipRegion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipPostalCode = table.Column<int>(type: "int", nullable: false),
                    ShipCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipInfo", x => x.ShipInfoId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_FullAddress_FullAddressId",
                        column: x => x.FullAddressId,
                        principalTable: "FullAddress",
                        principalColumn: "FullAddressId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShipInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_ShipInfo_ShipInfoId",
                        column: x => x.ShipInfoId,
                        principalTable: "ShipInfo",
                        principalColumn: "ShipInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_FullAddressId",
                table: "Customers",
                column: "FullAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShipInfoId",
                table: "Orders",
                column: "ShipInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "FullAddress");

            migrationBuilder.DropTable(
                name: "ShipInfo");
        }
    }
}

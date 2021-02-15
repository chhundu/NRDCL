using Microsoft.EntityFrameworkCore.Migrations;

namespace NRDCL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer_Table",
                columns: table => new
                {
                    CitizenshipID = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: true),
                    TelephoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    EmailId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer_Table", x => x.CitizenshipID);
                });

            migrationBuilder.CreateTable(
                name: "Product_Table",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductName = table.Column<string>(type: "TEXT", nullable: true),
                    PricePerUnit = table.Column<decimal>(type: "TEXT", nullable: false),
                    TransportRate = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Table", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Deposit_Table",
                columns: table => new
                {
                    CustomerID = table.Column<string>(type: "TEXT", nullable: false),
                    LastAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Balance = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposit_Table", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_Deposit_Table_Customer_Table_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer_Table",
                        principalColumn: "CitizenshipID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Site_Table",
                columns: table => new
                {
                    SiteId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CitizenshipID = table.Column<string>(type: "TEXT", nullable: true),
                    SiteName = table.Column<string>(type: "TEXT", nullable: true),
                    DistanceFrom = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Site_Table", x => x.SiteId);
                    table.ForeignKey(
                        name: "FK_Site_Table_Customer_Table_CitizenshipID",
                        column: x => x.CitizenshipID,
                        principalTable: "Customer_Table",
                        principalColumn: "CitizenshipID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order_Table",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerID = table.Column<string>(type: "TEXT", nullable: true),
                    SiteID = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductID = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    OrderAmount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Table", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Order_Table_Customer_Table_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer_Table",
                        principalColumn: "CitizenshipID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Table_Product_Table_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product_Table",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Table_Site_Table_SiteID",
                        column: x => x.SiteID,
                        principalTable: "Site_Table",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_Table_CustomerID",
                table: "Order_Table",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Table_ProductID",
                table: "Order_Table",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Table_SiteID",
                table: "Order_Table",
                column: "SiteID");

            migrationBuilder.CreateIndex(
                name: "IX_Site_Table_CitizenshipID",
                table: "Site_Table",
                column: "CitizenshipID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deposit_Table");

            migrationBuilder.DropTable(
                name: "Order_Table");

            migrationBuilder.DropTable(
                name: "Product_Table");

            migrationBuilder.DropTable(
                name: "Site_Table");

            migrationBuilder.DropTable(
                name: "Customer_Table");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace NRDCL.Migrations
{
    public partial class AddDBToDockerInstance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Table_Customer_Table_CustomerID",
                table: "Order_Table");

            migrationBuilder.DropForeignKey(
                name: "FK_Site_Table_Customer_Table_CitizenshipID",
                table: "Site_Table");

            migrationBuilder.AlterColumn<string>(
                name: "SiteName",
                table: "Site_Table",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CitizenshipID",
                table: "Site_Table",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Product_Table",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerID",
                table: "Order_Table",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TelephoneNumber",
                table: "Customer_Table",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "Customer_Table",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Table_Customer_Table_CustomerID",
                table: "Order_Table",
                column: "CustomerID",
                principalTable: "Customer_Table",
                principalColumn: "CitizenshipID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Site_Table_Customer_Table_CitizenshipID",
                table: "Site_Table",
                column: "CitizenshipID",
                principalTable: "Customer_Table",
                principalColumn: "CitizenshipID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Table_Customer_Table_CustomerID",
                table: "Order_Table");

            migrationBuilder.DropForeignKey(
                name: "FK_Site_Table_Customer_Table_CitizenshipID",
                table: "Site_Table");

            migrationBuilder.AlterColumn<string>(
                name: "SiteName",
                table: "Site_Table",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CitizenshipID",
                table: "Site_Table",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Product_Table",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerID",
                table: "Order_Table",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "TelephoneNumber",
                table: "Customer_Table",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "Customer_Table",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Table_Customer_Table_CustomerID",
                table: "Order_Table",
                column: "CustomerID",
                principalTable: "Customer_Table",
                principalColumn: "CitizenshipID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Site_Table_Customer_Table_CitizenshipID",
                table: "Site_Table",
                column: "CitizenshipID",
                principalTable: "Customer_Table",
                principalColumn: "CitizenshipID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kvitto.Data.Migrations
{
    public partial class noconstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadedFile_Receipt_ReceiptId",
                table: "UploadedFile");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiptId",
                table: "UploadedFile",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadedFile_Receipt_ReceiptId",
                table: "UploadedFile",
                column: "ReceiptId",
                principalTable: "Receipt",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadedFile_Receipt_ReceiptId",
                table: "UploadedFile");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiptId",
                table: "UploadedFile",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UploadedFile_Receipt_ReceiptId",
                table: "UploadedFile",
                column: "ReceiptId",
                principalTable: "Receipt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

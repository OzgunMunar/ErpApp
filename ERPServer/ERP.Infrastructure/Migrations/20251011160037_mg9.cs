using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceNumberDate",
                table: "Invoices",
                newName: "InvoiceDate");

            migrationBuilder.AddColumn<Guid>(
                name: "DepotId",
                table: "InvoiceDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_DepotId",
                table: "InvoiceDetails",
                column: "DepotId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Depots_DepotId",
                table: "InvoiceDetails",
                column: "DepotId",
                principalTable: "Depots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Depots_DepotId",
                table: "InvoiceDetails");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceDetails_DepotId",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "DepotId",
                table: "InvoiceDetails");

            migrationBuilder.RenameColumn(
                name: "InvoiceDate",
                table: "Invoices",
                newName: "InvoiceNumberDate");
        }
    }
}

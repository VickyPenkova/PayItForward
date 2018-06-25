using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PayItForward.Data.Migrations
{
    public partial class ChangeUserCollumnAvilableMoneyAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AvilableMoneyAmount",
                table: "AspNetUsers",
                type: "money",
                nullable: false,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "AvilableMoneyAmount",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");
        }
    }
}

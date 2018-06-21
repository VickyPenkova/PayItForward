using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PayItForward.Data.Migrations
{
    public partial class ChangeStoryDescriptionColumnRestrictionToVarchar1000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Stories",
                type: "varchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Stories",
                type: "varchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldNullable: true);
        }
    }
}

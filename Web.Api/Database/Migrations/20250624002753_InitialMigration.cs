using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Api.Database.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "customers",
            columns: table => new
            {
                id = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                identification_number = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                birth_date = table.Column<DateOnly>(type: "date", nullable: true),
                phone_number = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                status = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_customers", x => x.id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Customers_Email",
            table: "customers",
            column: "email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Customers_PhoneNumber",
            table: "customers",
            column: "phone_number",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "customers");
    }
}

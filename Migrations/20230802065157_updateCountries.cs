using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeekelApi.Migrations
{
    public partial class updateCountries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Countries",
                newName: "Capital");

            migrationBuilder.AddColumn<string>(
                name: "Alfa2Code",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Alfa3Code",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alfa2Code",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Alfa3Code",
                table: "Countries");

            migrationBuilder.RenameColumn(
                name: "Capital",
                table: "Countries",
                newName: "Code");
        }
    }
}

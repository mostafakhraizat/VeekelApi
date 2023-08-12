using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeekelApi.Migrations
{
    public partial class updateCountries1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.RenameColumn(
                name: "Alfa2Code",
                table: "Countries",
                newName: "Alpha3Code");

            migrationBuilder.AddColumn<string>(
                name: "Alpha2Code",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alpha2Code",
                table: "Countries");

        

            migrationBuilder.RenameColumn(
                name: "Alpha3Code",
                table: "Countries",
                newName: "Alfa2Code");
        }
    }
}

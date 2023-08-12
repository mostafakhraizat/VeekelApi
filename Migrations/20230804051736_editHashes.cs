using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeekelApi.Migrations
{
    public partial class editHashes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencoesHash",
                table: "ConfigHashes",
                newName: "CurrenciesHash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrenciesHash",
                table: "ConfigHashes",
                newName: "CurrencoesHash");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diia.Challenge.DAL.Migrations
{
    public partial class modified_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    CityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityDistrictId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}

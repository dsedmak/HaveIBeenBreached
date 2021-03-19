using Microsoft.EntityFrameworkCore.Migrations;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.EfCoreEmailAddressCollectionAdapter.Migrations
{
    public partial class Initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "EmailAddresses",
                table => new {Address = table.Column<string>("TEXT", maxLength: 254, nullable: false)},
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAddresses", x => x.Address);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "EmailAddresses");
        }
    }
}
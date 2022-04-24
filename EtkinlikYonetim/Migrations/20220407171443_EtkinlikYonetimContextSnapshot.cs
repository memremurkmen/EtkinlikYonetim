using Microsoft.EntityFrameworkCore.Migrations;

namespace EtkinlikYonetim.Migrations
{
    public partial class EtkinlikYonetimContextSnapshot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isDeleted",
                table: "EY_etkinlik",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<bool>(
            //    name: "isDeleted",
            //    table: "EY_etkinlik",
            //    type: "bit",
            //    nullable: true,
            //    oldClrType: typeof(bool),
            //    oldType: "bit");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EtkinlikYonetim.Migrations
{
    public partial class EtkinlikYonetimMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "EY_etkinlik",
            //    columns: table => new
            //    {
            //        etkinlikID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        etkinlikAdi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
            //        maxKatilimciSayisi = table.Column<int>(type: "int", nullable: false),
            //        baslangicTarihi = table.Column<DateTime>(type: "datetime", nullable: false),
            //        bitisTarihi = table.Column<DateTime>(type: "datetime", nullable: false),
            //        updateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        deleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        isDeleted = table.Column<bool>(type: "bit", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_EY_etkinlik", x => x.etkinlikID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "EY_kullanici",
            //    columns: table => new
            //    {
            //        kullaniciID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        telefonNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
            //        tcNo = table.Column<string>(type: "nchar(11)", fixedLength: true, maxLength: 11, nullable: false),
            //        sifre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        yetki = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_EY_kullanici", x => x.kullaniciID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "EY_etkinlikKullaniciEslesme",
            //    columns: table => new
            //    {
            //        etkinlikKullaniciEslesmeID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        etkinlikId = table.Column<int>(type: "int", nullable: true),
            //        kullaniciId = table.Column<int>(type: "int", nullable: true),
            //        girisYapildi = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_EY_etkinlikKullaniciEslesme", x => x.etkinlikKullaniciEslesmeID);
            //        table.ForeignKey(
            //            name: "FK_EY_etkinlikKullaniciEslesme_EY_etkinlik",
            //            column: x => x.etkinlikId,
            //            principalTable: "EY_etkinlik",
            //            principalColumn: "etkinlikID",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_EY_etkinlikKullaniciEslesme_EY_kullanici",
            //            column: x => x.kullaniciId,
            //            principalTable: "EY_kullanici",
            //            principalColumn: "kullaniciID",
            //            onDelete: ReferentialAction.SetNull);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_EY_etkinlikKullaniciEslesme_etkinlikId",
            //    table: "EY_etkinlikKullaniciEslesme",
            //    column: "etkinlikId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_EY_etkinlikKullaniciEslesme_kullaniciId",
            //    table: "EY_etkinlikKullaniciEslesme",
            //    column: "kullaniciId");

            //migrationBuilder.CreateIndex(
            //    name: "UQ__EY_kulla__E078675086B74C3F",
            //    table: "EY_kullanici",
            //    column: "tcNo",
            //    unique: true);
            
         //   migrationBuilder.AddColumn<DateTime>(
         //name: "updateDate",
         //table: "EY_etkinlik",
         //nullable: true);
         //   migrationBuilder.AddColumn<DateTime>(
         //name: "deleteDate",
         //table: "EY_etkinlik",
         //nullable: true);
         //   migrationBuilder.AddColumn<bool>(
         //name: "isDeleted",
         //table: "EY_etkinlik",
         //nullable: true);
            migrationBuilder.AddColumn<bool>(
         name: "girisYapildi",
         table: "EY_etkinlikKullaniciEslesme",
         nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
         //   migrationBuilder.AddColumn<DateTime>(
         //name: "updateDate",
         //table: "EY_etkinlik",
         //nullable: true);
         //   migrationBuilder.AddColumn<DateTime>(
         //name: "deleteDate",
         //table: "EY_etkinlik",
         //nullable: true);
         //   migrationBuilder.AddColumn<bool>(
         //name: "isDeleted",
         //table: "EY_etkinlik",
         //nullable: true);
            migrationBuilder.AddColumn<bool>(
         name: "girisYapildi",
         table: "EY_etkinlikKullaniciEslesme",
         nullable: true);
            //migrationBuilder.DropTable(
            //    name: "EY_etkinlikKullaniciEslesme");

            //migrationBuilder.DropTable(
            //    name: "EY_etkinlik");

            //migrationBuilder.DropTable(
            //    name: "EY_kullanici");
        }
    }
}

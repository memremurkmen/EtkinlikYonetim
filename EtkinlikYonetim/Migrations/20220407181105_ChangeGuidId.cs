using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EtkinlikYonetim.Migrations
{
    public partial class ChangeGuidId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_kullaniciID",
            //    table: "EY_kullanici"
            //    );
            migrationBuilder.DropColumn(
                name: "kullaniciID",
                table: "EY_kullanici"
                );
            migrationBuilder.AddColumn<Guid>(
                name: "kullaniciID",
                table: "EY_kullanici",
                type: "uniqueidentifier",
                nullable: false
                );
            migrationBuilder.AddPrimaryKey(
                name: "PK_kullaniciID",
                table: "EY_kullanici",
                column: "kullaniciID"
                );
            //migrationBuilder.AlterColumn<Guid>(
            //    name: "kullaniciID",
            //    table: "EY_kullanici",
            //    type: "uniqueidentifier",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");


            migrationBuilder.DropColumn(
                name: "kullaniciId",
                table: "EY_etkinlikKullaniciEslesme"
                );
            migrationBuilder.AddColumn<Guid>(
                name: "kullaniciId",
                table: "EY_etkinlikKullaniciEslesme",
                type: "uniqueidentifier",
                nullable: false
                );
            //migrationBuilder.AlterColumn<Guid>(
            //    name: "kullaniciId",
            //    table: "EY_etkinlikKullaniciEslesme",
            //    type: "uniqueidentifier",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);


            migrationBuilder.DropColumn(
                name: "etkinlikId",
                table: "EY_etkinlikKullaniciEslesme"
                );
            migrationBuilder.AddColumn<Guid>(
                name: "etkinlikId",
                table: "EY_etkinlikKullaniciEslesme",
                type: "uniqueidentifier",
                nullable: false
                );
            //migrationBuilder.AlterColumn<Guid>(
            //    name: "etkinlikId",
            //    table: "EY_etkinlikKullaniciEslesme",
            //    type: "uniqueidentifier",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);


            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_etkinlikKullaniciEslesmeID",
            //    table: "EY_etkinlikKullaniciEslesme");
            migrationBuilder.DropColumn(
                name: "etkinlikKullaniciEslesmeID",
                table: "EY_etkinlikKullaniciEslesme"
                );
            migrationBuilder.AddColumn<Guid>(
                name: "etkinlikKullaniciEslesmeID",
                table: "EY_etkinlikKullaniciEslesme",
                type: "uniqueidentifier",
                nullable: false
                );
            migrationBuilder.AddPrimaryKey(
                name: "PK_etkinlikKullaniciEslesmeID",
                table: "EY_etkinlikKullaniciEslesme",
                column: "etkinlikKullaniciEslesmeID"
                );
            //migrationBuilder.AlterColumn<Guid>(
            //    name: "etkinlikKullaniciEslesmeID",
            //    table: "EY_etkinlikKullaniciEslesme",
            //    type: "uniqueidentifier",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");


            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_etkinlikID",
            //    table: "EY_etkinlik"
            //    );
            migrationBuilder.DropColumn(
                name: "etkinlikID",
                table: "EY_etkinlik"
                );
            migrationBuilder.AddColumn<Guid>(
                name: "etkinlikID",
                table: "EY_etkinlik",
                type: "uniqueidentifier",
                nullable: false
                );
            migrationBuilder.AddPrimaryKey(
                name: "PK_etkinlikID",
                table: "EY_etkinlik",
                column: "etkinlikID"
                );
            //migrationBuilder.AlterColumn<Guid>(
            //    name: "etkinlikID",
            //    table: "EY_etkinlik",
            //    type: "uniqueidentifier",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_EY_kullanici_tcNo",
                table: "EY_kullanici",
                column: "tcNo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EY_etkinlikKullaniciEslesme_EY_kullanici_kullaniciId",
                table: "EY_etkinlikKullaniciEslesme",
                column: "kullaniciId",
                principalTable: "EY_kullanici",
                principalColumn: "kullaniciID",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_EY_etkinlikKullaniciEslesme_EY_etkinlik_etkinlikId",
                table: "EY_etkinlikKullaniciEslesme",
                column: "etkinlikId",
                principalTable: "EY_etkinlik",
                principalColumn: "etkinlikID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EY_kullanici_tcNo",
                table: "EY_kullanici");

            migrationBuilder.AlterColumn<int>(
                name: "kullaniciID",
                table: "EY_kullanici",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "kullaniciId",
                table: "EY_etkinlikKullaniciEslesme",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "etkinlikId",
                table: "EY_etkinlikKullaniciEslesme",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "etkinlikKullaniciEslesmeID",
                table: "EY_etkinlikKullaniciEslesme",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "etkinlikID",
                table: "EY_etkinlik",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}

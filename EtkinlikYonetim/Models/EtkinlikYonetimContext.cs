using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EtkinlikYonetim.Models
{
    public partial class EtkinlikYonetimContext : DbContext
    {
        public EtkinlikYonetimContext()
        {
        }

        public EtkinlikYonetimContext(DbContextOptions<EtkinlikYonetimContext> options) : base(options)
        {
        }

        public virtual DbSet<EyEtkinlik> EyEtkinlik { get; set; }
        public virtual DbSet<EyEtkinlikKullaniciEslesme> EyEtkinlikKullaniciEslesme { get; set; }
        public virtual DbSet<EyKullanici> EyKullanici { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=EMRE-PC\\SQLEXPRESS;Database=EtkinlikYonetim;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Turkish_CI_AS");

            modelBuilder.Entity<EyEtkinlik>(entity =>
            {
                entity.HasKey(e => e.EtkinlikId);

                entity.ToTable("EY_etkinlik");

                entity.Property(e => e.EtkinlikId).HasColumnName("etkinlikID");

                entity.Property(e => e.BaslangicTarihi)
                    .HasColumnType("datetime")
                    .HasColumnName("baslangicTarihi");

                entity.Property(e => e.BitisTarihi)
                    .HasColumnType("datetime")
                    .HasColumnName("bitisTarihi");

                entity.Property(e => e.EtkinlikAdi)
                    .HasMaxLength(200)
                    .HasColumnName("etkinlikAdi");

                entity.Property(e => e.MaxKatilimciSayisi).HasColumnName("maxKatilimciSayisi");
            });

            modelBuilder.Entity<EyEtkinlikKullaniciEslesme>(entity =>
            {
                entity.HasKey(e => e.EtkinlikKullaniciEslesmeId);

                entity.ToTable("EY_etkinlikKullaniciEslesme");

                entity.Property(e => e.EtkinlikKullaniciEslesmeId).HasColumnName("etkinlikKullaniciEslesmeID");

                entity.Property(e => e.EtkinlikId).HasColumnName("etkinlikId");

                entity.Property(e => e.KullaniciId).HasColumnName("kullaniciId");

                entity.HasOne(d => d.Etkinlik)
                    .WithMany(p => p.EyEtkinlikKullaniciEslesmes)
                    .HasForeignKey(d => d.EtkinlikId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EY_etkinlikKullaniciEslesme_EY_etkinlik");

                entity.HasOne(d => d.Kullanici)
                    .WithMany(p => p.EyEtkinlikKullaniciEslesmes)
                    .HasForeignKey(d => d.KullaniciId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EY_etkinlikKullaniciEslesme_EY_kullanici");
            });

            modelBuilder.Entity<EyKullanici>(entity =>
            {
                entity.HasKey(e => e.KullaniciId);

                entity.ToTable("EY_kullanici");

                entity.HasIndex(e => e.TcNo, "UQ__EY_kulla__E078675086B74C3F")
                    .IsUnique();

                entity.Property(e => e.KullaniciId).HasColumnName("kullaniciID");

                entity.Property(e => e.Ad)
                    .HasMaxLength(50)
                    .HasColumnName("ad");

                entity.Property(e => e.Sifre)
                    .HasMaxLength(50)
                    .HasColumnName("sifre");

                entity.Property(e => e.Soyad)
                    .HasMaxLength(50)
                    .HasColumnName("soyad");

                entity.Property(e => e.TcNo)
                    .HasMaxLength(11)
                    .HasColumnName("tcNo")
                    .IsFixedLength(true);

                entity.HasIndex(e => e.TcNo).IsUnique();

                entity.Property(e => e.TelefonNo)
                    .HasMaxLength(10)
                    .HasColumnName("telefonNo");

                entity.Property(e => e.Yetki)
                    .HasMaxLength(50)
                    .HasColumnName("yetki");
            });

            
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}

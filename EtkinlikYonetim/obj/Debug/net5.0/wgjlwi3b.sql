IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [EY_etkinlik] (
    [etkinlikID] int NOT NULL IDENTITY,
    [etkinlikAdi] nvarchar(200) NULL,
    [maxKatilimciSayisi] int NULL,
    [baslangicTarihi] datetime NULL,
    [bitisTarihi] datetime NULL,
    CONSTRAINT [PK_EY_etkinlik] PRIMARY KEY ([etkinlikID])
);
GO

CREATE TABLE [EY_kullanici] (
    [kullaniciID] int NOT NULL IDENTITY,
    [ad] nvarchar(50) NULL,
    [soyad] nvarchar(50) NULL,
    [telefonNo] nvarchar(10) NULL,
    [tcNo] nchar(11) NULL,
    [sifre] nvarchar(50) NULL,
    [yetki] nvarchar(50) NULL,
    CONSTRAINT [PK_EY_kullanici] PRIMARY KEY ([kullaniciID])
);
GO

CREATE TABLE [EY_etkinlikKullaniciEslesme] (
    [etkinlikKullaniciEslesmeID] int NOT NULL IDENTITY,
    [etkinlikId] int NULL,
    [kullaniciId] int NULL,
    CONSTRAINT [PK_EY_etkinlikKullaniciEslesme] PRIMARY KEY ([etkinlikKullaniciEslesmeID]),
    CONSTRAINT [FK_EY_etkinlikKullaniciEslesme_EY_etkinlik] FOREIGN KEY ([etkinlikId]) REFERENCES [EY_etkinlik] ([etkinlikID]) ON DELETE CASCADE,
    CONSTRAINT [FK_EY_etkinlikKullaniciEslesme_EY_kullanici] FOREIGN KEY ([kullaniciId]) REFERENCES [EY_kullanici] ([kullaniciID]) ON DELETE SET NULL
);
GO

CREATE INDEX [IX_EY_etkinlikKullaniciEslesme_etkinlikId] ON [EY_etkinlikKullaniciEslesme] ([etkinlikId]);
GO

CREATE INDEX [IX_EY_etkinlikKullaniciEslesme_kullaniciId] ON [EY_etkinlikKullaniciEslesme] ([kullaniciId]);
GO

CREATE UNIQUE INDEX [UQ__EY_kulla__E078675086B74C3F] ON [EY_kullanici] ([tcNo]) WHERE [tcNo] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220329123200_SP_etkinlikIDyeGoreKatilimcilar', N'5.0.15');
GO

COMMIT;
GO


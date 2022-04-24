using EtkinlikYonetim.Models;
using EtkinlikYonetim.QrCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using ZXing;
using ZXing.Windows.Compatibility;
using EtkinlikYonetim.Functions;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace EtkinlikYonetim.Controllers
{
    public class EtkinlikController : Controller
    {
        EtkinlikYonetimContext db = new EtkinlikYonetimContext();

        [HttpGet]
        [Authorize(Roles = "Admin,Yetkili,Personel")]
        public ActionResult QrKodOlustur(Guid id)
        {
            string qrText = id.ToString() + "_" + HttpContext.Session.GetString("kullaniciId");
            AesCrypto aes = new AesCrypto();
            string qrCryptedText = aes.EncryptString(qrText);//qr koduna koyulacak metni şifreleme
            var image = QrCodeGenerator.GenerateByteArray(qrCryptedText);//kullanıcıId ve etkinlikId yi qr kod içerisine koyma
            return File(image, "image/jpeg");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult QrCodeRead(Guid id)
        {
            ViewBag.etkinlikId = id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Yetkili")]
        public async Task<IActionResult> QrCodeRead(List<IFormFile> files,Guid etkinlikId)
        {
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(formFile.FileName, FileMode.Create))//viewdan gelen dosyayı proje dosyası içine kaydetme
                    {
                        await formFile.CopyToAsync(stream);//gelen dosya işlenebilmesi için proje dosyalarının içine kopyalanıyor
                    }
                    var reader = new BarcodeReaderGeneric();
                    Bitmap image = null;
                    try
                    {
                        image = (Bitmap)Image.FromFile(formFile.FileName);//dosyayı bitmap(image) formatına çevirme
                    }
                    catch (Exception)
                    {
                        ViewBag.QrText = "Geçersiz formatta bir dosya yüklenmeye çalışıldı!";
                    }
                    if (image != null)
                    {
                        using (image)
                        {
                            LuminanceSource source = new BitmapLuminanceSource(image);
                            Result result = reader.Decode(source);//qr kodu decode etme
                            if (result != null)
                            {
                                AesCrypto aes = new AesCrypto();
                                var qrDecryptedText = aes.DecryptString(result.Text);//şifreli qr kodunu çözme
                                var qrTextSplit = qrDecryptedText.Split('_');
                                EyEtkinlikKullaniciEslesme kullaniciQrKontrol = db.EyEtkinlikKullaniciEslesme.Where(a => a.EtkinlikId == etkinlikId
                                                                                            && a.KullaniciId == Guid.Parse(qrTextSplit[1])).FirstOrDefault();//veritabanından kullanıcıId ve etkinlikId davetliler tablosunda eşleşiyor mu diye bakılıyor
                                if (kullaniciQrKontrol != null && qrTextSplit[0] == etkinlikId.ToString())//hem veritabanında eşleşme varsa hem de viewda seçili etkinlikse onay verilecek
                                {
                                    kullaniciQrKontrol.girisYapildi = true;
                                    db.SaveChanges();
                                    ViewBag.QrText = "Kullanıcı bu etkinliğe davetlidir. Giriş yapıldı.";
                                }
                                else
                                {
                                    ViewBag.QrText = "Kullanıcı bu etkinliğe davetli değildir. Giremez!";
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.QrText = "Lütfen geçerli bir dosya yükleyiniz!";
                    }
                    FileInfo fileInfo = new FileInfo(formFile.FileName);//proje dosyalarına kaydedilen qr kod alınıyor
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();//proje dosyalarında gereksiz doluluk olmaması için işi biten qr kod siliniyor
                    }
                }
            }
            return View("QrCodeRead");
        }


        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult Etkinlikler()
        {
            var etkinlikler = db.EyEtkinlik.OrderBy(c => c.BaslangicTarihi).ToList();
            return View(etkinlikler);
        }

        [Authorize(Roles = "Admin, Yetkili, Personel")]
        public ActionResult Etkinliklerim()
        {
            var etkinlikler = (from ek in db.EyEtkinlikKullaniciEslesme
                               join e in db.EyEtkinlik
                               on ek.EtkinlikId equals e.EtkinlikId
                               join k in db.EyKullanici
                               on ek.KullaniciId equals k.KullaniciId
                               where ek.KullaniciId == Guid.Parse(HttpContext.Session.GetString("kullaniciId")) &&
                               e.isDeleted == false
                               select new EyEtkinlikKullaniciEslesme
                               {
                                   Etkinlik = e,
                                   Kullanici = k,
                                   girisYapildi = ek.girisYapildi
                               }).OrderByDescending(c => c.Etkinlik.BaslangicTarihi).ToList();
            return View(etkinlikler);
        }


        // GET: EtkinlikController/Details/5
        //[Authorize(Roles = "Admin, Yetkili, Personel")]
        //public ActionResult EtkinlikDetay(Guid id)
        //{
        //    HttpContext.Session.SetString("duzenlenenEtkinlikId", id.ToString());
        //    return View(db.EyEtkinlik.Find(id));
        //}

        // GET: EtkinlikController/Create
        //[Authorize(Roles = "Admin,Yetkili")]
        //public ActionResult EtkinlikOlustur()
        //{
        //    return View();
        //}

        //// POST: EtkinlikController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin,Yetkili")]
        //public ActionResult EtkinlikOlustur(EyEtkinlik yeniEtkinlik)
        //{
        //    yeniEtkinlik.updateDate = DateTime.Now;
        //    db.EyEtkinlik.Add(yeniEtkinlik);
        //    db.SaveChanges();
        //    try
        //    {
        //        return RedirectToAction(nameof(Etkinlikler));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        [AcceptVerbs("Post")]
        public ActionResult EditingPopup_CreateEtkinlik([DataSourceRequest] DataSourceRequest request, EyEtkinlik yeniEtkinlik)
        {
            if (yeniEtkinlik != null)
            {
                yeniEtkinlik.updateDate = DateTime.Now;
                yeniEtkinlik.deleteDate = null;
                db.EyEtkinlik.Add(yeniEtkinlik);
                db.SaveChanges();
            }

            return Json(new[] { yeniEtkinlik }.ToDataSourceResult(request));
        }

        //[AcceptVerbs("Post")]
        //public ActionResult Editing_CreateEtkinlik([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<EyEtkinlik> yeniEtkinlikler)
        //{
        //    var results = new List<EyEtkinlik>();

        //    if (yeniEtkinlikler != null)
        //    {
        //        foreach (var etkinlik in yeniEtkinlikler)
        //        {
        //            etkinlik.updateDate = DateTime.Now;
        //            db.EyEtkinlik.Add(etkinlik);
        //            db.SaveChanges();
        //            results.Add(etkinlik);
        //        }
        //    }
        //    return Json(results.ToDataSourceResult(request, ModelState));
        //}

        //[AcceptVerbs("Post")]
        //public ActionResult Editing_UpdateEtkinlik([DataSourceRequest] DataSourceRequest request, EyEtkinlik yeniEtkinlik)
        //{
        //    if (yeniEtkinlik != null)
        //    {
        //        var etkinlik = db.EyEtkinlik.Find(yeniEtkinlik.EtkinlikId);//düzenlenecek olan etkinlik çekilip belirli sütunları güncelleniyor.
        //        etkinlik.EtkinlikAdi = yeniEtkinlik.EtkinlikAdi;
        //        etkinlik.MaxKatilimciSayisi = yeniEtkinlik.MaxKatilimciSayisi;
        //        etkinlik.BaslangicTarihi = yeniEtkinlik.BaslangicTarihi;
        //        etkinlik.BitisTarihi = yeniEtkinlik.BitisTarihi;
        //        etkinlik.updateDate = DateTime.Now;
        //        yeniEtkinlik.updateDate = DateTime.Now;
        //        db.SaveChanges();
        //    }

        //    return Json(new[] { yeniEtkinlik }.ToDataSourceResult(request, ModelState));
        //}



        // GET: EtkinlikController/Edit/5
        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult EtkinlikDuzenle(Guid id)
        {
            HttpContext.Session.SetString("duzenlenenEtkinlikId", id.ToString());
            EtkinlikDuzenleModel vm = new EtkinlikDuzenleModel();//viewa birden fazla veritabanı nesnesi göndereceğimizden yeni bir model oluşturdum
            vm.activitiesUsers = (from ek in db.EyEtkinlikKullaniciEslesme
                                  join e in db.EyEtkinlik
                                  on ek.EtkinlikId equals e.EtkinlikId
                                  join k in db.EyKullanici
                                  on ek.KullaniciId equals k.KullaniciId
                                  where ek.EtkinlikId == id
                                  select new EyEtkinlikKullaniciEslesme
                                  {
                                      Etkinlik = e,
                                      Kullanici = k,
                                      girisYapildi = ek.girisYapildi
                                  }).ToList();
            vm.activity = db.EyEtkinlik.Find(id);
            vm.users = db.EyKullanici.ToList();

            return View(vm);//hem seçilen etkinliğin davetli kullanıcılarını hem de bütün kullanıcı bilgilerini göndermek icin viewmodel oluşturdum
        }

        // POST: EtkinlikController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult EtkinlikDuzenle(Guid id, EtkinlikDuzenleModel newActivity)
        {
            var etkinlik = db.EyEtkinlik.Find(id);//düzenlenecek olan etkinlik çekilip belirli sütunları güncelleniyor.
            etkinlik.EtkinlikAdi = newActivity.activity.EtkinlikAdi;
            etkinlik.MaxKatilimciSayisi = newActivity.activity.MaxKatilimciSayisi;
            etkinlik.BaslangicTarihi = newActivity.activity.BaslangicTarihi;
            etkinlik.BitisTarihi = newActivity.activity.BitisTarihi;
            etkinlik.updateDate = DateTime.Now;
            db.SaveChanges();
            try
            {
                return RedirectToAction(nameof(Etkinlikler));
            }
            catch
            {
                return View();
            }
        }

        [AcceptVerbs("Post")]
        public ActionResult EditingPopup_UpdateEtkinlik([DataSourceRequest] DataSourceRequest request, EyEtkinlik yeniEtkinlik)
        {
            if (yeniEtkinlik != null)
            {
                var etkinlik = db.EyEtkinlik.Find(yeniEtkinlik.EtkinlikId);//düzenlenecek olan etkinlik çekilip belirli sütunları güncelleniyor.
                etkinlik.EtkinlikAdi = yeniEtkinlik.EtkinlikAdi;
                etkinlik.MaxKatilimciSayisi = yeniEtkinlik.MaxKatilimciSayisi;
                etkinlik.BaslangicTarihi = yeniEtkinlik.BaslangicTarihi;
                etkinlik.BitisTarihi = yeniEtkinlik.BitisTarihi;
                etkinlik.updateDate = DateTime.Now;
                db.SaveChanges();
            }

            return Json(new[] { yeniEtkinlik }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult DavetliEkle(Guid davetliId, string etkinlikId)
        {
            Guid etkinlikIdGuid = Guid.Parse(etkinlikId);
            List<EyEtkinlikKullaniciEslesme> etkinlikDetay = (from ek in db.EyEtkinlikKullaniciEslesme
                                                              join e in db.EyEtkinlik
                                                              on ek.EtkinlikId equals e.EtkinlikId
                                                              join k in db.EyKullanici
                                                              on ek.KullaniciId equals k.KullaniciId
                                                              where ek.EtkinlikId == etkinlikIdGuid
                                                              select new EyEtkinlikKullaniciEslesme
                                                              {
                                                                  Etkinlik = e,
                                                                  Kullanici = k
                                                              }).ToList();//davetli listesi çekiliyor

            foreach (var item in etkinlikDetay)
            {
                if (item.Kullanici.KullaniciId == davetliId)//etkinlikte davet edilecek kullanıcı zaten davetli mi diye kontrol ediliyor
                {
                    return RedirectToAction("EtkinlikDuzenle", "Etkinlik", new { id = etkinlikIdGuid });
                }
            }
            if (etkinlikDetay.Count() == 0 || etkinlikDetay.Count() < etkinlikDetay[0].Etkinlik.MaxKatilimciSayisi)//etkinliğe eklenebilecek max katılımcı sayısı geçiliyor mu diye kontrol ediliyor
            {
                EyEtkinlikKullaniciEslesme eslesme = new EyEtkinlikKullaniciEslesme();
                eslesme.KullaniciId = davetliId;
                eslesme.EtkinlikId = etkinlikIdGuid;
                eslesme.girisYapildi = false;
                db.EyEtkinlikKullaniciEslesme.Add(eslesme);
                db.SaveChanges();
            }
            return RedirectToAction("EtkinlikDuzenle", "Etkinlik", new { id = etkinlikIdGuid });
        }

        //[AcceptVerbs("Post")]
        //[Authorize(Roles = "Admin,Yetkili")]
        //public ActionResult DavetliEkle([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<EyKullanici> davetli)
        //{
        //    var results = new List<EyKullanici>();
        //    results.Add(davetli.First());
        //    if (davetli != null)
        //    {
        //        List<EyEtkinlikKullaniciEslesme> etkinlikDetay = (from ek in db.EyEtkinlikKullaniciEslesme
        //                                                          join e in db.EyEtkinlik
        //                                                          on ek.EtkinlikId equals e.EtkinlikId
        //                                                          join k in db.EyKullanici
        //                                                          on ek.KullaniciId equals k.KullaniciId
        //                                                          where ek.EtkinlikId == Guid.Parse(HttpContext.Session.GetString("duzenlenenEtkinlikId"))
        //                                                          select new EyEtkinlikKullaniciEslesme
        //                                                          {
        //                                                              Etkinlik = e,
        //                                                              Kullanici = k
        //                                                          }).ToList();//davetli listesi çekiliyor

        //        foreach (var item in etkinlikDetay)
        //        {
        //            if (item.Kullanici.KullaniciId == davetli.First().KullaniciId)//etkinlikte davet edilecek kullanıcı zaten davetli mi diye kontrol ediliyor
        //            {
        //                return Json(results.ToDataSourceResult(request, ModelState));
        //            }
        //        }
        //        if (etkinlikDetay.Count() == 0 || etkinlikDetay.Count() < etkinlikDetay[0].Etkinlik.MaxKatilimciSayisi)//etkinliğe eklenebilecek max katılımcı sayısı geçiliyor mu diye kontrol ediliyor
        //        {
        //            EyEtkinlikKullaniciEslesme eslesme = new EyEtkinlikKullaniciEslesme();
        //            eslesme.KullaniciId = davetli.First().KullaniciId;
        //            eslesme.EtkinlikId = Guid.Parse(HttpContext.Session.GetString("duzenlenenEtkinlikId"));
        //            eslesme.girisYapildi = false;
        //            db.EyEtkinlikKullaniciEslesme.Add(eslesme);
        //            db.SaveChanges();
        //        }
        //    }

        //    return Json(results.ToDataSourceResult(request, ModelState));
        //}

        [HttpPost]
        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult DavetliSil(Guid kullaniciId, Guid etkinlikId)//etkinlikten davetli silme
        {
            var eslesme = db.EyEtkinlikKullaniciEslesme.Where(a => a.KullaniciId == kullaniciId && a.EtkinlikId == etkinlikId).FirstOrDefault();
            db.EyEtkinlikKullaniciEslesme.Remove(eslesme);
            db.SaveChanges();
            try
            {
                return Json(ModelState.ToDataSourceResult());
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult EtkinlikSil([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            var deletedActicity = db.EyEtkinlik.Find(id);//veritabanından etkinlik silinmiyor. sadece görünürlüğü kapatılıyor.
            deletedActicity.deleteDate = DateTime.Now;
            deletedActicity.isDeleted = true;
            db.SaveChanges();
            try
            {
                return Json(ModelState.ToDataSourceResult());
            }
            catch
            {
                return View();
            }
        }
    }
}

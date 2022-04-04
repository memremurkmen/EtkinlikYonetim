using EtkinlikYonetim.Models;
using EtkinlikYonetim.QrCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtkinlikYonetim.Controllers
{
    public class EtkinlikController : Controller
    {
        EtkinlikYonetimContext db = new EtkinlikYonetimContext();

        [HttpGet]
        [Authorize(Roles = "Admin,Yetkili,Personel")]
        public ActionResult QrKodOlustur(int id)
        {
            string qrSifre = "Ad:" + HttpContext.Session.GetString("kullaniciAdi") +
                " Soyad:" + HttpContext.Session.GetString("kullaniciSoyadi") +
                " TC:" + HttpContext.Session.GetString("kullaniciTc") +
                " Etkinlik ID:" + id;
            var image = QrCodeGenerator.GenerateByteArray(qrSifre);
            return File(image, "image/jpeg");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult QrKodOkut(int id)
        {
            return RedirectToAction("EtkinlikDetay", "Etkinlik", new { id = id });
        }


        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult Etkinlikler()
        {
            List<EyEtkinlik> etkinlikler = db.EyEtkinlik.OrderByDescending(c => c.BaslangicTarihi).ToList();
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
                               where ek.KullaniciId == HttpContext.Session.GetInt32("kullaniciId") &&
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
        [Authorize(Roles = "Admin, Yetkili, Personel")]
        public ActionResult EtkinlikDetay(int id)
        {
            return View(db.EyEtkinlik.Find(id));
        }

        // GET: EtkinlikController/Create
        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult EtkinlikOlustur()
        {
            return View();
        }

        // POST: EtkinlikController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult EtkinlikOlustur(EyEtkinlik yeniEtkinlik)
        {
            yeniEtkinlik.updateDate = DateTime.Now;
            db.EyEtkinlik.Add(yeniEtkinlik);
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

        // GET: EtkinlikController/Edit/5
        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult EtkinlikDuzenle(int id)
        {
            HttpContext.Session.SetInt32("duzenlenenEtkinlikId", id);
            EtkinlikDuzenleModel vm = new EtkinlikDuzenleModel();
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
        public ActionResult EtkinlikDuzenle(int id, EtkinlikDuzenleModel newActivity)
        {
            var etkinlik = db.EyEtkinlik.Find(id);
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

        [HttpPost]
        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult DavetliEkle(int davetliId)
        {
            List<EyEtkinlikKullaniciEslesme> etkinlikDetay = (from ek in db.EyEtkinlikKullaniciEslesme
                                                              join e in db.EyEtkinlik
                                                              on ek.EtkinlikId equals e.EtkinlikId
                                                              join k in db.EyKullanici
                                                              on ek.KullaniciId equals k.KullaniciId
                                                              where ek.EtkinlikId == HttpContext.Session.GetInt32("duzenlenenEtkinlikId")
                                                              select new EyEtkinlikKullaniciEslesme
                                                              {
                                                                  Etkinlik = e,
                                                                  Kullanici = k
                                                              }).ToList();

            foreach (var item in etkinlikDetay)
            {
                if (item.Kullanici.KullaniciId == davetliId)
                {
                    return RedirectToAction("EtkinlikDuzenle", "Etkinlik", new { id = HttpContext.Session.GetInt32("duzenlenenEtkinlikId") });
                }
            }
            if (etkinlikDetay.Count() == 0 || etkinlikDetay.Count() < etkinlikDetay[0].Etkinlik.MaxKatilimciSayisi)
            {
                EyEtkinlikKullaniciEslesme eslesme = new EyEtkinlikKullaniciEslesme();
                eslesme.KullaniciId = davetliId;
                eslesme.EtkinlikId = HttpContext.Session.GetInt32("duzenlenenEtkinlikId");
                eslesme.girisYapildi = false;
                db.EyEtkinlikKullaniciEslesme.Add(eslesme);
                db.SaveChanges();
            }
            return RedirectToAction("EtkinlikDuzenle", "Etkinlik", new { id = HttpContext.Session.GetInt32("duzenlenenEtkinlikId") });
        }
        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult DavetliSil(int id)
        {
            var eslesme = db.EyEtkinlikKullaniciEslesme.Where(a => a.KullaniciId == id && a.EtkinlikId == HttpContext.Session.GetInt32("duzenlenenEtkinlikId")).FirstOrDefault();
            db.EyEtkinlikKullaniciEslesme.Remove(eslesme);
            db.SaveChanges();
            return RedirectToAction("EtkinlikDuzenle", "Etkinlik", new { id = eslesme.EtkinlikId });
        }

        // GET: EtkinlikController/Delete/5
        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult EtkinlikSil(int id)
        {
            return View(db.EyEtkinlik.Find(id));
        }

        // POST: EtkinlikController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Yetkili")]
        public ActionResult EtkinlikSil(int id, IFormCollection collection)
        {
            var deletedActicity = db.EyEtkinlik.Find(id);
            deletedActicity.deleteDate = DateTime.Now;
            deletedActicity.isDeleted = true;
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
    }
}

using EtkinlikYonetim.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtkinlikYonetim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KullaniciController : Controller
    {
        EtkinlikYonetimContext db = new EtkinlikYonetimContext();
        // GET: KullaniciController
        public ActionResult Kullanicilar()
        {
            List<EyKullanici> kullanicilar = db.EyKullanici.ToList();
            return View(kullanicilar);
        }

        // GET: KullaniciController/Details/5
        public ActionResult KullaniciDetay(int id)
        {
            return View(db.EyKullanici.Find(id));
        }

        // GET: KullaniciController/Create
        public ActionResult KullaniciOlustur()
        {
            return View();
        }

        // POST: KullaniciController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KullaniciOlustur(EyKullanici yeniKullanici)
        {
            var kullaniciKayitKontrol = db.EyKullanici.Any(a => a.TelefonNo == yeniKullanici.TelefonNo
            || a.TcNo == yeniKullanici.TcNo);
            if (kullaniciKayitKontrol)
            {
                return View("KullaniciOlustur");
            }
            db.EyKullanici.Add(yeniKullanici);
            db.SaveChanges();
            try
            {
                return RedirectToAction(nameof(Kullanicilar));
            }
            catch
            {
                return View();
            }
        }

        // GET: KullaniciController/Edit/5
        public ActionResult KullaniciDuzenle(int id)
        {
            return View(db.EyKullanici.Find(id));
        }

        // POST: KullaniciController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KullaniciDuzenle(int id, EyKullanici yeniKullanici)
        {
            var kullanici = db.EyKullanici.Find(id);
            kullanici.Ad = yeniKullanici.Ad;
            kullanici.Soyad = yeniKullanici.Soyad;
            kullanici.TelefonNo = yeniKullanici.TelefonNo;
            kullanici.TcNo = yeniKullanici.TcNo;
            kullanici.Sifre = yeniKullanici.Sifre;
            kullanici.Yetki = yeniKullanici.Yetki;
            db.SaveChanges();
            try
            {
                return RedirectToAction(nameof(Kullanicilar));
            }
            catch
            {
                return View();
            }
        }

        // GET: KullaniciController/Delete/5
        public ActionResult KullaniciSil(int id)
        {
            return View(db.EyKullanici.Find(id));
        }

        // POST: KullaniciController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KullaniciSil(int id, IFormCollection collection)
        {
            db.EyKullanici.Remove(db.EyKullanici.Find(id));
            db.SaveChanges();
            try
            {
                return RedirectToAction(nameof(Kullanicilar));
            }
            catch
            {
                return View();
            }
        }
    }
}

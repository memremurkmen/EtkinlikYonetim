using EtkinlikYonetim.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtkinlikYonetim.Controllers
{
    public class User : Controller
    {
        EtkinlikYonetimContext db = new EtkinlikYonetimContext();
        public ActionResult Hesabım()
        {
            return View(db.EyKullanici.Find(Guid.Parse(HttpContext.Session.GetString("kullaniciId"))));//sessionda tutulan kullanıcıId sine göre veriyi viewa gönderiyor
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Hesabım(EyKullanici yeniKullanici)
        {
            var eskiKullanici = db.EyKullanici.Where(a => a.KullaniciId == yeniKullanici.KullaniciId).FirstOrDefault();
            bool kullaniciTelefonNoKontrol = db.EyKullanici.Any(a => a.TelefonNo == yeniKullanici.TelefonNo && a.TelefonNo != eskiKullanici.TelefonNo);
            bool kullaniciTcNoKontrol = db.EyKullanici.Any(a => a.TcNo == yeniKullanici.TcNo && a.TcNo != eskiKullanici.TcNo);
            if (kullaniciTelefonNoKontrol || kullaniciTcNoKontrol)//kullanıcının değiştirdiği telefonNo veya tcNo başka kullanıcıyla aynı mı diye kontrol ediliyor.
            {
                return View("Hesabım");
            }
            eskiKullanici.Ad = yeniKullanici.Ad;
            eskiKullanici.Soyad = yeniKullanici.Soyad;
            eskiKullanici.TelefonNo = yeniKullanici.TelefonNo;
            eskiKullanici.TcNo = yeniKullanici.TcNo;
            eskiKullanici.Sifre = yeniKullanici.Sifre;
            db.SaveChanges();
            HttpContext.Session.SetString("kullaniciAdi", yeniKullanici.Ad);
            HttpContext.Session.SetString("kullaniciSoyadi", yeniKullanici.Soyad);
            HttpContext.Session.SetString("kullaniciTc", yeniKullanici.TcNo);
            try
            {
                return RedirectToAction("","");
            }
            catch
            {
                return View();
            }
        }
    }
}

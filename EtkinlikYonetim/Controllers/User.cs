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
            return View(db.EyKullanici.Find(HttpContext.Session.GetInt32("kullaniciId")));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Hesabım(EyKullanici yeniKullanici)
        {
            var eskiKullanici = db.EyKullanici.Where(a => a.KullaniciId == yeniKullanici.KullaniciId).FirstOrDefault();
            var kullaniciTelefonNoKontrol = db.EyKullanici.Where(a => a.TelefonNo == yeniKullanici.TelefonNo).FirstOrDefault();
            if (kullaniciTelefonNoKontrol != null && eskiKullanici.TelefonNo != kullaniciTelefonNoKontrol.TelefonNo)
            {
                return View("Hesabım");
            }
            eskiKullanici.Ad = yeniKullanici.Ad;
            eskiKullanici.Soyad = yeniKullanici.Soyad;
            eskiKullanici.TelefonNo = yeniKullanici.TelefonNo;
            eskiKullanici.Sifre = yeniKullanici.Sifre;
            db.SaveChanges();
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

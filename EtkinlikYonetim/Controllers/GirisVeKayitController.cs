using EtkinlikYonetim.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Providers.Entities;

namespace EtkinlikYonetim.Controllers
{
    public class GirisVeKayitController : Controller
    {
        EtkinlikYonetimContext db = new EtkinlikYonetimContext();

        [AllowAnonymous]
        public ActionResult GirisYap()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GirisYapAsync(EyKullanici k)
        {
            var kullanici = db.EyKullanici.Where(a => a.TcNo == k.TcNo && a.Sifre == k.Sifre).FirstOrDefault();
            if (kullanici != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,kullanici.TcNo),
                    new Claim(ClaimTypes.Role,kullanici.Yetki)
                };
                var kullaniciIdentity = new ClaimsIdentity(claims, "a");
                ClaimsPrincipal principal = new ClaimsPrincipal(kullaniciIdentity);
                await HttpContext.SignInAsync(principal);

                HttpContext.Session.SetInt32("kullaniciId", kullanici.KullaniciId);
                HttpContext.Session.SetString("kullaniciAdi", kullanici.Ad);
                HttpContext.Session.SetString("kullaniciSoyadi", kullanici.Soyad);
                HttpContext.Session.SetString("kullaniciTc", kullanici.TcNo);
                HttpContext.Session.SetString("kullaniciYetki", kullanici.Yetki);

                return RedirectToAction("Index", "Home");//ana sayfaya yönlendirilecek
            }
            else
            {
                return View();
            }
        }



        [AllowAnonymous]
        public ActionResult KayitOl()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KayitOl(EyKullanici k)
        {
            var kullaniciKayitKontrol = db.EyKullanici.Any(a => a.TelefonNo == k.TelefonNo || a.TcNo == k.TcNo);
            if (kullaniciKayitKontrol)
            {
                return View("KayitOl");
            }

            k.Yetki = "Personel";

            db.EyKullanici.Add(k);
            db.SaveChanges();
            return RedirectToAction("GirisYap", "GirisVeKayit");
        }


        public async Task<ActionResult> CikisYapAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("GirisYap", "GirisVeKayit");
        }
    }
}

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
        public async Task<ActionResult> GirisYap()
        {
            if (Request.Cookies["kullaniciId"] != null)
            {
                var kullanici = db.EyKullanici.Where(a => a.KullaniciId == Guid.Parse(Request.Cookies["kullaniciId"])).FirstOrDefault();
                if (kullanici != null)
                {
                    var claims = new List<Claim>//authorize için yetki veriliyor
                    {
                        new Claim(ClaimTypes.Name,kullanici.TcNo),
                        new Claim(ClaimTypes.Role,kullanici.Yetki)
                    };
                    var kullaniciIdentity = new ClaimsIdentity(claims, "a");
                    ClaimsPrincipal principal = new ClaimsPrincipal(kullaniciIdentity);
                    await HttpContext.SignInAsync(principal);
                    HttpContext.Session.SetString("kullaniciId", kullanici.KullaniciId.ToString());//diğer controllerlarda kullanılmak üzere sessionlar kaydediliyor.
                    HttpContext.Session.SetString("kullaniciAdi", kullanici.Ad);
                    HttpContext.Session.SetString("kullaniciSoyadi", kullanici.Soyad);
                    HttpContext.Session.SetString("kullaniciTc", kullanici.TcNo);
                    HttpContext.Session.SetString("kullaniciYetki", kullanici.Yetki);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }

            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GirisYap(EyKullanici k)
        {

            var kullanici = db.EyKullanici.Where(a => a.TcNo == k.TcNo && a.Sifre == k.Sifre).FirstOrDefault();//viewdan gelen tc ve şifre veritabanındaki kullanıcılardan birisiyle eşleşiyor mu diye bakılıyor
            if (kullanici != null)
            {
                var claims = new List<Claim>//authorize için yetki veriliyor
                {
                    new Claim(ClaimTypes.Name,kullanici.TcNo),
                    new Claim(ClaimTypes.Role,kullanici.Yetki)
                };
                var kullaniciIdentity = new ClaimsIdentity(claims, "a");
                ClaimsPrincipal principal = new ClaimsPrincipal(kullaniciIdentity);
                await HttpContext.SignInAsync(principal);

                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(2);
                Response.Cookies.Append("kullaniciId", kullanici.KullaniciId.ToString(), options);

                HttpContext.Session.SetString("kullaniciId", kullanici.KullaniciId.ToString());//diğer controllerlarda kullanılmak üzere sessionlar kaydediliyor.
                HttpContext.Session.SetString("kullaniciAdi", kullanici.Ad);
                HttpContext.Session.SetString("kullaniciSoyadi", kullanici.Soyad);
                HttpContext.Session.SetString("kullaniciTc", kullanici.TcNo);
                HttpContext.Session.SetString("kullaniciYetki", kullanici.Yetki);

                return RedirectToAction("Index", "Home");
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
            var kullaniciKayitKontrol = db.EyKullanici.Any(a => a.TelefonNo == k.TelefonNo || a.TcNo == k.TcNo);//aynı telefonNo veya tcNo var mı diye kontrol ediliyor.
            if (kullaniciKayitKontrol)
            {
                return View("KayitOl");
            }

            k.Yetki = "Personel";//kayıt olan kullanıcıya otomatik personel yetkisi veriliyor.

            db.EyKullanici.Add(k);
            db.SaveChanges();
            return RedirectToAction("GirisYap", "GirisVeKayit");
        }


        public async Task<ActionResult> CikisYapAsync()
        {
            Response.Cookies.Delete("kullaniciId");
            await HttpContext.SignOutAsync();
            return RedirectToAction("GirisYap", "GirisVeKayit");
        }
    }
}

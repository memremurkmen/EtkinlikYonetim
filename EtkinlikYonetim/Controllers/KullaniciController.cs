using EtkinlikYonetim.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace EtkinlikYonetim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KullaniciController : Controller
    {
        EtkinlikYonetimContext db = new EtkinlikYonetimContext();
        // GET: KullaniciController

        public ActionResult Kullanicilar()
        {
            var kullanicilar = db.EyKullanici.ToList();
            return View(kullanicilar);
        }
        public ActionResult HierarchyBinding_Kullanicilar([DataSourceRequest] DataSourceRequest request, Guid etkinlikId)
        {
            var kullanicilar = (from ek in db.EyEtkinlikKullaniciEslesme
                                join e in db.EyEtkinlik
                                on ek.EtkinlikId equals e.EtkinlikId
                                join k in db.EyKullanici
                                on ek.KullaniciId equals k.KullaniciId
                                where ek.EtkinlikId == etkinlikId
                                select new EyEtkinlikKullaniciEslesme
                                {
                                    Etkinlik = e,
                                    Kullanici = k,
                                    girisYapildi = ek.girisYapildi
                                }).ToList();

            return Json(kullanicilar.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult GetAllKullanici()
        {
            var kullanicilar = db.EyKullanici.ToList();

            return Json(kullanicilar);
        }

        // GET: KullaniciController/Details/5
        public ActionResult KullaniciDetay(Guid id)
        {
            return View(db.EyKullanici.Find(id));
        }


        //// GET: KullaniciController/Create
        //public ActionResult KullaniciOlustur()
        //{
        //    return View();
        //}

        //// POST: KullaniciController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult KullaniciOlustur(EyKullanici yeniKullanici)
        //{
        //    var kullaniciKayitKontrol = db.EyKullanici.Any(a => a.TelefonNo == yeniKullanici.TelefonNo
        //    || a.TcNo == yeniKullanici.TcNo);//veritabanında aynı telefonNo veya TcNo var mı diye kontrol ediliyor
        //    if (kullaniciKayitKontrol)
        //    {
        //        return View("KullaniciOlustur");
        //    }
        //    db.EyKullanici.Add(yeniKullanici);
        //    db.SaveChanges();
        //    try
        //    {
        //        return RedirectToAction(nameof(Kullanicilar));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        [AcceptVerbs("Post")]
        public ActionResult EditingInLine_CreateKullanici([DataSourceRequest] DataSourceRequest request, EyKullanici yeniKullanici)
        {
            if (yeniKullanici != null)
            {
                var kullaniciKayitKontrol = db.EyKullanici.Any(a => a.TelefonNo == yeniKullanici.TelefonNo
                || a.TcNo == yeniKullanici.TcNo);//veritabanında aynı telefonNo veya TcNo var mı diye kontrol ediliyor
                if (kullaniciKayitKontrol)
                {
                    return View();
                }
                db.EyKullanici.Add(yeniKullanici);
                db.SaveChanges();
            }
            
            return Json(new[] { yeniKullanici }.ToDataSourceResult(request));
        }

        //// GET: KullaniciController/Edit/5
        //public ActionResult KullaniciDuzenle(Guid id)
        //{
        //    return View(db.EyKullanici.Find(id));
        //}

        //// POST: KullaniciController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult KullaniciDuzenle(EyKullanici yeniKullanici)
        //{
        //    var kullanici = db.EyKullanici.Find(yeniKullanici.KullaniciId);
        //    bool kullaniciTelefonNoKontrol = db.EyKullanici.Any(a => a.TelefonNo == yeniKullanici.TelefonNo && a.TelefonNo != kullanici.TelefonNo);
        //    bool kullaniciTcNoKontrol = db.EyKullanici.Any(a => a.TcNo == yeniKullanici.TcNo && a.TcNo != kullanici.TcNo);
        //    if (kullaniciTelefonNoKontrol || kullaniciTcNoKontrol)//kullanıcının değiştirdiği telefonNo veya tcNo başka kullanıcıyla aynı mı diye kontrol ediliyor.
        //    {
        //        return RedirectToAction("KullaniciDuzenle", "Kullanici", new { id = yeniKullanici.KullaniciId });
        //    }
        //    kullanici.Ad = yeniKullanici.Ad;
        //    kullanici.Soyad = yeniKullanici.Soyad;
        //    kullanici.TelefonNo = yeniKullanici.TelefonNo;
        //    kullanici.TcNo = yeniKullanici.TcNo;
        //    kullanici.Sifre = yeniKullanici.Sifre;
        //    kullanici.Yetki = yeniKullanici.Yetki;
        //    db.SaveChanges();
        //    try
        //    {
        //        return RedirectToAction(nameof(Kullanicilar));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        [AcceptVerbs("Post")]
        public ActionResult EditingInLine_UpdateKullanici([DataSourceRequest] DataSourceRequest request, EyKullanici yeniKullanici)
        {
            if (yeniKullanici != null)
            {
                var kullanici = db.EyKullanici.Find(yeniKullanici.KullaniciId);
                bool kullaniciTelefonNoKontrol = db.EyKullanici.Any(a => a.TelefonNo == yeniKullanici.TelefonNo && a.TelefonNo != kullanici.TelefonNo);
                bool kullaniciTcNoKontrol = db.EyKullanici.Any(a => a.TcNo == yeniKullanici.TcNo && a.TcNo != kullanici.TcNo);
                if (kullaniciTelefonNoKontrol || kullaniciTcNoKontrol)//kullanıcının değiştirdiği telefonNo veya tcNo başka kullanıcıyla aynı mı diye kontrol ediliyor.
                {
                    return View();
                }
                kullanici.Ad = yeniKullanici.Ad;
                kullanici.Soyad = yeniKullanici.Soyad;
                kullanici.TelefonNo = yeniKullanici.TelefonNo;
                kullanici.TcNo = yeniKullanici.TcNo;
                kullanici.Sifre = yeniKullanici.Sifre;
                kullanici.Yetki = yeniKullanici.Yetki;
                db.SaveChanges();
            }

            return Json(new[] { yeniKullanici }.ToDataSourceResult(request, ModelState));
        }


        // POST: KullaniciController/Delete/5
        [HttpPost]
        public ActionResult KullaniciSil([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            db.EyKullanici.Remove(db.EyKullanici.Find(id));
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

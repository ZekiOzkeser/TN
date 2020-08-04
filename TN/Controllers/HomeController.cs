using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TN.BLL;
using TN.DAL;
using TN.Models;
using TN.Models.Extended;

namespace TN.Controllers
{
    public class HomeController : Controller
    {
        TeklifContext db = new TeklifContext();
        GenericRepository<Kategoriler> cat = new GenericRepository<Kategoriler>(new TeklifContext());
        GenericRepository<Resimler> rsm = new GenericRepository<Resimler>(new TeklifContext());
        GenericRepository<Urunler> urn = new GenericRepository<Urunler>(new TeklifContext());
        
        [Route("AnaSayfa")]
        [Route("")]
        public ActionResult Index()
        {
            try
            {
                if (Request.Cookies["cerezim"] != null || Session["user"]!=null)
                {
                    HttpCookie cerez = Request.Cookies["cerezim"];
                    var x = cerez.Values["Email"];
                    var d = db.Users.Where(p => p.EmailID == x).FirstOrDefault();
                    Session["user"] = d;
                    if (d==null)
                    {
                        var ct = db.Urunlers.Where(m=>m.Yayın==true).OrderByDescending(t =>t.Bakilma).Take(3);
                        return View(ct);
                    }
                    return RedirectToAction("UrunIndex", "Kullanici");
                }

                var c = db.Urunlers.Where(m => m.Yayın == true).OrderByDescending(x => x.Bakilma).Take(3);
                return View(c);
            }
            catch (Exception ex)
            {
                ViewBag.hata = ex;
                return RedirectToAction("", "Home");
            }

        }

        [Route("acikartirma/suankapali")]
        public ActionResult AcikArtirma()
        {
            return View();
        }

        public ActionResult Urun1()
        {
            var d = db.Urunlers.Where(x=>x.Yayın==true).OrderBy(x => x.Kategoriler.KategoriAdi).ToList();
            return View(d);
        }
        public ActionResult Urun2()
        {
            var d = db.Urunlers.Where(x => x.Yayın == true).OrderByDescending(x => x.UrunOlusturma).ToList();
            return View(d);
        }
        public ActionResult Urun3()
        {
            var d = db.Urunlers.Where(x => x.Yayın == true).OrderByDescending(x => x.Durum.Durumu).ToList();
            return View(d);
        }
        public ActionResult Urun4()
        {
            var d = db.Urunlers.Where(x => x.Yayın == true).OrderByDescending(x => x.Bakilma).ToList();
            return View(d);
        }


        [Route("urun/={id}/detay")]
        public ActionResult Detay(int id)
        {
            AnaSayfa a = new AnaSayfa();
            a.Resimlers = db.Resimlers.Where(x => x.UrunId == id).ToList();
            a.Urunler = db.Urunlers.Where(x => x.UrunId == id).SingleOrDefault();
            int c = a.Urunler.UserID;
            a.Iletisim = db.Iletisims.Where(x => x.UserID == c).SingleOrDefault();
            int il = a.Iletisim.IlId;
            int ilce = a.Iletisim.ID;
            a.Il = db.Il.Where(x => x.IlId == il).SingleOrDefault();
            a.Ilce = db.Ilce.Where(x => x.ID == ilce).SingleOrDefault();
            Urunler urun = urn.Bring(id);
            urun.Bakilma += 1;
            urn.Edit(urun);
            return View(a);
        }

        public ActionResult HeaderDesktop()
        {
            var vm = db.Kategorilers.ToList();
            return View(vm);
        }

        public ActionResult HeaderMobile()
        {
            var vm = db.Kategorilers.ToList();
            return View(vm);
        }

        public ActionResult Footer()
        {
            TumEkranlar tum = new TumEkranlar();
            tum.Kategorilers = db.Kategorilers.ToList().Take(6);
            tum.Urunlers = db.Urunlers.Where(m=>m.Yayın==true).ToList().OrderByDescending(x => x.Baslik).Take(5);
            return View(tum);
        }

      
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                cat.Dispose();
                rsm.Dispose();
                urn.Dispose();
            }

            base.Dispose(disposing);
        }

    }
}
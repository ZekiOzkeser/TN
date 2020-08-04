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
    public class IlanController : Controller
    {
        TeklifContext db = new TeklifContext();
        GenericRepository<Kategoriler> cat = new GenericRepository<Kategoriler>(new TeklifContext());
        GenericRepository<Resimler> rsm = new GenericRepository<Resimler>(new TeklifContext());
        GenericRepository<Urunler> urn = new GenericRepository<Urunler>(new TeklifContext());
        public ActionResult Ilan(int ilanNo)
        {
            var urun1 = db.Urunlers.Where(x => x.UrunId == (ilanNo)).SingleOrDefault();

            if (urun1 == null)
            {
                TempData["msg"] = "Gecersiz ilan numarası...";
                return RedirectToAction("Index", "Home");
            }
            else if (urun1.Yayın == false)
            {
                TempData["msg"] = "İlan yayın da değil...";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                AnaSayfa a = new AnaSayfa();
                a.Resimlers = db.Resimlers.Where(x => x.UrunId == ilanNo).ToList();
                a.Urunler = db.Urunlers.Where(x => x.UrunId == ilanNo).SingleOrDefault();
                int c = a.Urunler.UserID;
                a.Iletisim = db.Iletisims.Where(x => x.UserID == c).SingleOrDefault();
                int il = a.Iletisim.IlId;
                int ilce = a.Iletisim.ID;
                a.Il = db.Il.Where(x => x.IlId == il).SingleOrDefault();
                a.Ilce = db.Ilce.Where(x => x.ID == ilce).SingleOrDefault();
                Urunler urun = urn.Bring(ilanNo);

                if (urun != null)
                {
                    urun.Bakilma += 1;
                    urn.Edit(urun);
                }

                return View(a);
            }
        }

        [Authorize]
        public ActionResult AramaSonuc(int Ara)
        {
            var urun1 = db.Urunlers.Where(x => x.UrunId == (Ara)).SingleOrDefault();

            if (urun1 == null)
            {
                TempData["msg"] = "Gecersiz ilan numarası...";
                return RedirectToAction("UrunIndex", "Kullanici");
            }
            else if (urun1.Yayın == false)
            {
                TempData["msg"] = "İlan yayın da değil...";
                return RedirectToAction("UrunIndex", "Kullanici");
            }
            else
            {
                AnaSayfa a = new AnaSayfa();
                a.Resimlers = db.Resimlers.Where(x => x.UrunId == Ara).ToList();
                a.Urunler = db.Urunlers.Where(x => x.UrunId == Ara).SingleOrDefault();
                int c = a.Urunler.UserID;
                a.Iletisim = db.Iletisims.Where(x => x.UserID == c).SingleOrDefault();
                int il = a.Iletisim.IlId;
                int ilce = a.Iletisim.ID;
                a.Il = db.Il.Where(x => x.IlId == il).SingleOrDefault();
                a.Ilce = db.Ilce.Where(x => x.ID == ilce).SingleOrDefault();
                Urunler urun = urn.Bring(Ara);

                if (urun != null)
                {
                    urun.Bakilma += 1;
                    urn.Edit(urun);
                }
                return View(a);
            }
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
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

    public class AdminController : Controller
    {
        TeklifContext db = new TeklifContext();
        GenericRepository<User> kul = new GenericRepository<User>(new TeklifContext());
        GenericRepository<Urunler> urn = new GenericRepository<Urunler>(new TeklifContext());
        GenericRepository<Kategoriler> cat = new GenericRepository<Kategoriler>(new TeklifContext());
        GenericRepository<Durum> dr = new GenericRepository<Durum>(new TeklifContext());
        GenericRepository<Mesaj> msj = new GenericRepository<Mesaj>(new TeklifContext());
        GenericRepository<Teklifler> tek = new GenericRepository<Teklifler>(new TeklifContext());
       
        [Route("zek")]
        public ActionResult Index()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID=="zekiozkeser@teklifinedir.com")
                {
                    Session["kullanicilar"] = db.Users.Count();
                    Session["Mesajlar"] = db.Mesajs.Count();
                    Session["Urunler"] = db.Urunlers.Count();
                    Session["Teklifler"] = db.Tekliflers.Count();
                    int c= db.Urunlers.Select(x=>x.Bakilma).Count();
                    if (c!=0)
                    {
                        Session["bakilma"] = db.Urunlers.Sum(x => x.Bakilma);
                    }
                   
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }
            return RedirectToAction("PageError", "Home");
        }

        public ActionResult Desktop()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }
            return RedirectToAction("PageError", "Error");
        }

        public ActionResult Mobile()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }
            return RedirectToAction("PageError", "Error");

        }

        public ActionResult Kullanicilar()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {

                    return View(db.Users.ToList());
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");

        }

        public string KullaniciSil(int id)
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    try
                    {
                        kul.Delete(id);
                        return "1";
                    }
                    catch (Exception)
                    {

                        return "-1";
                    }
                }

            }

            return "PageError";

        }

        public ActionResult TumUrunler()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    var lst = db.Urunlers.OrderBy(u => u.Yayın == true).ToList();
                    return View(lst);
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");

        }

        public ActionResult Kategoriler()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    return View(db.Kategorilers.ToList());
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }

        public ActionResult KatDuz(int id)
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    return View(cat.Bring(id));
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }

        [HttpPost]
        public ActionResult KatDuz(Kategoriler kat)
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    cat.Edit(kat);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }
        public ActionResult KategoriEkle()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }

        [HttpPost]
        public ActionResult KategoriEkle(Kategoriler kat)
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    TempData["msg"] = cat.Create(kat) ? "Kategori Eklendi.." : "Hata: Lütfen Tekrar Deneyiniz..";
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }

        public ActionResult Durum()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {

                    return View(db.Durums.ToList());
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }

        public ActionResult DurumDuz(int id)
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    return View(dr.Bring(id));
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");

        }

        [HttpPost]
        public ActionResult DurumDuz(Durum drm)
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    dr.Edit(drm);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }
        public ActionResult DurumEkle()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }

        [HttpPost]
        public ActionResult DurumEkle(Durum drm)
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    TempData["msg"] = dr.Create(drm) ? "Durum Eklendi.." : "Hata: Lütfen Tekrar Deneyiniz..";
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }

        public ActionResult TumMesajlar()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    var lst = db.Mesajs.ToList();

                    return View(lst);
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }
        public ActionResult MesajSil(int id)
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {

                    msj.Delete(id);
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");


        }


        public ActionResult TumTeklifler()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    var lst = db.Tekliflers.ToList();

                    return View(lst);
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }
        public ActionResult TeklifSil(int id)
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {

                    tek.Delete(id);
                    return View();

                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }

        public string DurumSil(int id)
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    try
                    {
                        dr.Delete(id);
                        return "1";
                    }
                    catch (Exception)
                    {

                        return "-1";
                    }
                }

            }

            return "PageError";


        }

        public string KatSil(int id)
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    try
                    {
                        cat.Delete(id);
                        return "1";
                    }
                    catch (Exception)
                    {

                        return "-1";
                    }
                }

            }

            return "PageError";


        }
        public string UrunSil(int id)
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    try
                    {
                        urn.Delete(id);
                        return "1";
                    }
                    catch (Exception)
                    {

                        return "-1";
                    }
                }

            }

            return "PageError";

        }

        public ActionResult Aktif()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    var lst = db.Urunlers.Select(x => x.Yayın == true).ToList();
                    return View(lst);
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }
        public ActionResult Pasif()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com" || kullanici.EmailID == "zekiozkeser@teklifinedir.com")
                {
                    var lst = db.Urunlers.Select(x => x.Yayın == false).ToList();
                    return View(lst);
                }
                else
                {
                    return RedirectToAction("Index", "Kullanici");
                }
            }

            return RedirectToAction("PageError", "Error");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                kul.Dispose();
                urn.Dispose();
                cat.Dispose();
                dr.Dispose();
                msj.Dispose();
                tek.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
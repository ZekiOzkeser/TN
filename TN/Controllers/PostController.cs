
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using TN.BLL;
using TN.DAL;
using TN.Models;
using TN.Models.Extended;

namespace TeklifinNedir.Controllers
{
    [Authorize]

    public class PostController : Controller
    {
        // GET: Post
        GenericRepository<Kategoriler> cat = new GenericRepository<Kategoriler>(new TeklifContext());
        GenericRepository<Durum> drm = new GenericRepository<Durum>(new TeklifContext());
        GenericRepository<Urunler> urn = new GenericRepository<Urunler>(new TeklifContext());
        GenericRepository<Resimler> rsm = new GenericRepository<Resimler>(new TeklifContext());

        GenericRepository<AnaGorsel> ana = new GenericRepository<AnaGorsel>(new TeklifContext());
        TeklifContext db = new TeklifContext();

        public ActionResult Index()
        {
            ViewModel vm = new ViewModel();
            vm.Kategorilers = db.Kategorilers.ToList();
            vm.Urunlers = db.Urunlers.ToList();
            return View(vm);
        }



        public ActionResult UrunEkle()
        {
            if (Session["user"] is User kullanici)
            {
                var d = db.Iletisims.Where(m => m.UserID == kullanici.UserID).Count();
                int deger = Convert.ToInt16(d);
                if (deger == 0)
                {
                    TempData["msg"] = "Lütfen iletişim bilgilerinizi tamamlayınız !!";
                    return RedirectToAction("Iletisim", "User");
                }
                ViewBag.KategoriId = new SelectList(cat.Listele(), "KategoriId ", "KategoriAdi");
                ViewBag.DurumId = new SelectList(drm.Listele(), "DurumId", "Durumu");
                return View();
            }
            return RedirectToAction("Registration", "User");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UrunEkle(Urunler urun)
        {
            if (Session["user"] is User kullanici)
            {
                if (ModelState.IsValid)
                {
                    urun.UserID = kullanici.UserID;
                    urun.UrunOlusturma = DateTime.Today;
                    urun.Bakilma = 0;
                    urun.Yayın = true;
                    Session["urun"] = urun;
                    return RedirectToAction("ResimYukle", "Post");
                }
            }

            return View();
        }


        public ActionResult ResimYukle()
        {
            if (Session["user"] is User)
            {
                TumEkranlar tum = new TumEkranlar();
                tum.Durums = db.Durums.ToList();
                tum.Kategorilers = db.Kategorilers.ToList();
                return View(tum);
            }
            else
            {
                return RedirectToAction("UrunEkle", "Post");
            }

        }

        [HttpPost]
        public ActionResult ResimYukle(HttpPostedFileBase[] resimDeneme)
        {
            if (Session["urun"] is Urunler)
            {
                bool hata = false;
                string FileName = "";
                _ = Request.Files;
                List<Resimler> rslistesi = new List<Resimler>();

                foreach (HttpPostedFileBase item in resimDeneme)
                {
                    string fname;
                    fname = item.FileName;
                    FileName = item.FileName;

                    if (((item.InputStream.Length / 1024) / 1024) >= 5)
                    {
                        hata = true;
                        break;
                    }

                    FileInfo fileinfo = new FileInfo(item.FileName);
                    WebImage img = new WebImage(item.InputStream);
                    string uzanti = (Guid.NewGuid().ToString() + fileinfo.Extension).ToLower();
                    
                    img.Resize(1200, 900, true, false);
                    if (img.Height < 1200 || img.Width < 900)
                    {
                        img.AddTextWatermark("www.teklifiNedir.com", "#fff825", 15, "Regular", "Arial", "Center", "middle", 100, 50);
                    }
                    else
                    {
                        img.AddTextWatermark("www.teklifiNedir.com", "#fff825", 50, "Regular", "Arial", "Center", "middle", 100, 50);
                    }
                    string tamyol = "/Temp/" + uzanti;
                    img.Save(Server.MapPath(tamyol));
                    rslistesi.Add(
                                new Resimler()
                                {
                                    ImageUrl = tamyol,
                                    Resim = uzanti
                                });
                }

                TempData["hatali"] = hata;

                if (!hata)
                {
                    Session["res"] = rslistesi;
                    return RedirectToAction("AnaResim", "Post");
                }

                TempData["msg"] = "Dosya boyutu 5 mb.";
                return RedirectToAction("ResimYukle", "Post");
            }

            return View();

        }


        public ActionResult AnaResim()
        {
            TumEkranlar tum = new TumEkranlar();
            tum.Durums = db.Durums.ToList();
            tum.Kategorilers = db.Kategorilers.ToList();
            return View(tum);
        }



        [HttpPost]
        public ActionResult AnaResim(FormCollection c)
        {
            if (c["ImageUrl"] == null)
            {
                TumEkranlar tum = new TumEkranlar();
                tum.Durums = db.Durums.ToList();
                tum.Kategorilers = db.Kategorilers.ToList();
                ViewBag.msg = "Lütfen Görsel Seçiniz...";
                return View(tum);
            }

            Session["anagorsel"] = c["ImageUrl"].ToString();
            return RedirectToAction("Kontrol");
        }


        public ActionResult Kontrol()
        {
            TumEkranlar tum = new TumEkranlar();
            tum.Durums = db.Durums.ToList();
            tum.Kategorilers = db.Kategorilers.ToList();

            return View(tum);
        }

        [HttpPost]
        public ActionResult Kontrol(FormCollection c)
        {
            if (Session["user"] is User kullanici)
            {
                Urunler u = new Urunler();
                u.Aciklama = ((Urunler)Session["urun"]).Aciklama.ToString();
                u.AltLimit = ((Urunler)Session["urun"]).AltLimit;
                u.Bakilma = 0;
                u.Baslik = ((Urunler)Session["urun"]).Baslik.ToString();
                u.DurumId = ((Urunler)Session["urun"]).DurumId;
                u.KategoriId = ((Urunler)Session["urun"]).KategoriId;
                u.UrunOlusturma = DateTime.Now;
                u.UserID = kullanici.UserID;
                u.Yayın = true;
                var ur = urn.Create(u);

                var menu = Session["res"] as List<Resimler>;
                foreach (var item in menu)
                {
                    Resimler rs = new Resimler();
                    rs.ImageUrl = item.ImageUrl;
                    rs.Resim = item.Resim;
                    rs.UrunId = u.UrunId;
                    rsm.Create(rs);
                }


                string img = Session["anagorsel"].ToString();
                int lst = db.Resimlers.Where(x => x.ImageUrl == img).Select(x => x.ResimId).Single();

                AnaGorsel ana = new AnaGorsel();
                ana.ResimId = lst;
                db.AnaGorsels.Add(ana);
                db.SaveChanges();



                u.GorselId = ana.GorselId;
                TempData["msg"] = urn.Edit(u) ? "Ürününüz Yayında" : "Hata !! Lütfen Tekrar Deneyiniz...";

                return RedirectToAction("Index", "Kullanici");

            }
            return View();
        }

        public ActionResult UrunDuzenle(int id)
        {
            Urunler u = urn.Bring(id);
            ViewBag.KategoriId = new SelectList(cat.Listele(), "KategoriId ", "KategoriAdi", u.KategoriId);
            ViewBag.DurumId = new SelectList(drm.Listele(), "DurumId", "Durumu", u.DurumId);
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UrunDuzenle(Urunler urunler)
        {
            if (Session["user"] is User kullanici)
            {
                if (ModelState.IsValid)
                {
                    urunler.UserID = kullanici.UserID;
                    Session["ud"] = urunler;
                    urn.Edit(urunler);
                    return RedirectToAction("ResimDuzenle", "Post");
                }
            }
            return View();
        }


        public ActionResult ResimDuzenle()
        {
            if (Session["ud"] is Urunler u)
            {
                TumEkranlar tum = new TumEkranlar();
                tum.Resimlers = db.Resimlers.Where(m => m.UrunId == u.UrunId).ToList();
                tum.Durums = db.Durums.ToList();
                tum.Kategorilers = db.Kategorilers.ToList();
                return View(tum);
            }

            return RedirectToAction("UrunDuzenle", "Post");
        }

        [HttpPost]
        public ActionResult ResimDuzenle(HttpPostedFileBase[] resimDeneme2)
        {
            if (Session["ud"] is Urunler)
            {
                string FileName = "";
                HttpFileCollectionBase files = Request.Files;
                List<Resimler> rslistesi = new List<Resimler>();
                int sayac = 0;
                foreach (HttpPostedFileBase item in resimDeneme2)
                {
                    int i = sayac++;
                    HttpPostedFileBase file = files[i];
                    string fname;
                    fname = file.FileName;
                    FileName = file.FileName;
                    FileInfo fileinfo = new FileInfo(file.FileName);
                    WebImage img = new WebImage(file.InputStream);
                    string uzanti = (Guid.NewGuid().ToString() + fileinfo.Extension).ToLower();
                    img.Resize(1200, 900, true, false);
                    if (img.Height < 1200 || img.Width < 900)
                    {
                        img.AddTextWatermark("www.teklifiNedir.com", "#fff825", 15, "Regular", "Arial", "Center", "middle", 100, 50);
                    }
                    else
                    {
                        img.AddTextWatermark("www.teklifiNedir.com", "#fff825", 50, "Regular", "Arial", "Center", "middle", 100, 50);
                    }
                    string tamyol = "/Temp/" + uzanti;
                    img.Save(Server.MapPath(tamyol));
                    rslistesi.Add(
                                new Resimler()
                                {
                                    ImageUrl = tamyol,
                                    Resim = uzanti
                                });
                }
                Session["resduz"] = rslistesi;
                return RedirectToAction("AnaResimDuz", "Post");
            }
            return View();
        }

        public ActionResult AnaResimDuz()
        {
            TumEkranlar tum = new TumEkranlar();
            tum.Durums = db.Durums.ToList();
            tum.Kategorilers = db.Kategorilers.ToList();
            return View(tum);
        }

        [HttpPost]
        public ActionResult AnaResimDuz(FormCollection c)
        {
            if (c["ImageUrl"] == null)
            {
                TumEkranlar tum = new TumEkranlar();
                tum.Durums = db.Durums.ToList();
                tum.Kategorilers = db.Kategorilers.ToList();
                ViewBag.msg = "Lütfen Görsel Seçiniz...";
                return View(tum);
            }

            Session["gorselduz"] = c["ImageUrl"].ToString();
            return RedirectToAction("Kontrol2");
        }


        public ActionResult Kontrol2()
        {
            TumEkranlar tum = new TumEkranlar();
            tum.Durums = db.Durums.ToList();
            tum.Kategorilers = db.Kategorilers.ToList();
            return View(tum);
        }

        [HttpPost]
        public ActionResult Kontrol2(FormCollection c)
        {
            if (Session["user"] is User kullanici)
            {
                var u = ((Urunler)Session["ud"]);
                var menu = Session["resduz"] as List<Resimler>;
                foreach (var item in menu)
                {
                    Resimler rs = new Resimler();
                    rs.ImageUrl = item.ImageUrl;
                    rs.Resim = item.Resim;
                    rs.UrunId = u.UrunId;
                    rsm.Create(rs);
                }
                string img = Session["gorselduz"].ToString();
                int lst = db.Resimlers.Where(x => x.ImageUrl == img).Select(x => x.ResimId).Single();
                AnaGorsel ana = new AnaGorsel();
                ana.ResimId = lst;
                db.AnaGorsels.Add(ana);
                db.SaveChanges();
                u.GorselId = ana.GorselId;
                TempData["msg"] = urn.Edit(u) ? "Ürününüz Düzenlendi.." : "Hata !! Lütfen Tekrar Deneyiniz...";

                return RedirectToAction("Index", "Kullanici");

            }
            return View();
        }


        public PartialViewResult UrunDetay(int? id)
        {
            if (id != null)
            {
                var lst = db.Urunlers.Where(p => p.UrunId == id).Single();
                return PartialView(lst);
            }
            else
            {
                int d = ((Urunler)Session["urun"]).UrunId;
                var lst = db.Urunlers.Where(p => p.UrunId == d).Single();
                return PartialView(lst);
            }
        }

        public ActionResult RsmSil(int id)
        {
            rsm.Delete(id);
            return View();
        }
        public ActionResult ResimSil()
        {

            int d = ((Urunler)Session["urun"]).UrunId;
            IEnumerable<Resimler> lst = db.Resimlers.Where(m => m.UrunId == d).ToList();
            return View(lst);

        }

        [HttpPost]
        public ActionResult ResimSil(IEnumerable<int> ID)
        {
            foreach (var id in ID)
            {
                var image = db.Resimlers.Single(s => s.ResimId == id);
                string imgPath = Server.MapPath(image.ImageUrl);
                db.Resimlers.Remove(image);
                if (System.IO.File.Exists(imgPath))
                {
                    System.IO.File.Delete(imgPath);
                }
            }
            db.SaveChanges();
            return View();
        }


        [HttpPost]
        public ActionResult ResDis(IEnumerable<int> res)
        {
            foreach (var id in res)
            {
                var image = db.Resimlers.Single(s => s.ResimId == id);
                int a = db.AnaGorsels.Where(s => s.ResimId == id).Select(x => x.GorselId).FirstOrDefault();
                int b = db.Urunlers.Where(c => c.AnaGorsel.GorselId == a).Select(c => c.UrunId).SingleOrDefault();
                if (b != 0)
                {
                    var resdeg = urn.Bring(b);
                    resdeg.GorselId = null;
                    urn.Edit(resdeg);
                    string imgPath = Server.MapPath(image.ImageUrl);
                    db.Resimlers.Remove(image);
                    if (System.IO.File.Exists(imgPath))
                    {
                        System.IO.File.Delete(imgPath);
                    }
                    db.SaveChanges();

                }
                else
                {

                    string imgPath = Server.MapPath(image.ImageUrl);
                    db.Resimlers.Remove(image);
                    if (System.IO.File.Exists(imgPath))
                    {
                        System.IO.File.Delete(imgPath);
                    }
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ResimDuzenle", "Post");
        }

        public string UrunSil(int id)
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

        public string YayindanKaldir(int id)
        {
            try
            {
                var d = db.Urunlers.Where(m => m.UrunId == id).SingleOrDefault();
                Urunler u = new Urunler();
                u.Aciklama = d.Aciklama;
                u.AltLimit = d.AltLimit;
                u.Baslik = d.Baslik;
                u.DurumId = d.DurumId;
                u.KategoriId = d.KategoriId;
                u.UrunOlusturma = d.UrunOlusturma;
                u.UrunBitisTarihi = DateTime.Now;
                u.Yayın = false;
                u.UserID = d.UserID;
                u.Bakilma = d.Bakilma;
                u.GorselId = d.GorselId;
                u.UrunId = id;
                TempData["msg"] = urn.Edit(u) ? "Yayından kaldırıldı" : "Hata !! Lütfen daha sonra tekrar deneyiniz..";

                return "1";
            }
            catch (Exception)
            {

                return "-1";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                drm.Dispose();
                urn.Dispose();
                rsm.Dispose();
                cat.Dispose();
                ana.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
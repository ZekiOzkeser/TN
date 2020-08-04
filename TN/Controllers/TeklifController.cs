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
    public class TeklifController : Controller
    {
        TeklifContext db = new TeklifContext();
        GenericRepository<Teklifler> tek = new GenericRepository<Teklifler>(new TeklifContext());
        GenericRepository<Urunler> urn = new GenericRepository<Urunler>(new TeklifContext());
        GenericRepository<Mesaj> msg = new GenericRepository<Mesaj>(new TeklifContext());
        public ActionResult Index()
        {
            if (Session["user"] is User k)
                return View(db.Tekliflers.Where(x => x.Kime == k.UserID).OrderByDescending(x => x.TeklifTarihi).ToList());
           
            else
                return RedirectToAction("PageError", "Error");
           
        }

        public ActionResult TklVer(Teklifler tklf)
        {
            if (Session["user"] is User kullanici)
            {
                tklf.TeklifVeren = kullanici.UserID;
                tklf.TeklifTarihi = DateTime.Now;
                if (tklf.UrunId == 0)
                {
                    tklf.UrunId = Convert.ToInt32(Session["tekurun"]);
                    tklf.Kime = Convert.ToInt32(Session["tekgond"]);
                    //TempData["msg"] = tek.Create(tklf) ? "Teklife mesajınız iletildi.." : "Hata !! Lütfen Tekrar Deneyiniz..";

                    return PartialView(tek.Create(tklf));
                }
                else if (tklf.Kime == 0)
                {
                    tklf.Kime = Convert.ToInt32(Session["tekgond"]);
                    if (tklf.Kime==tklf.TeklifVeren)
                    {
                        tklf.Kime = Convert.ToInt32(Session["sonteklif"]);
                        //TempData["msg"] = tek.Create(tklf) ? "Teklife mesajınız iletildi.." : "Hata !! Lütfen Tekrar Deneyiniz..";
                        return View(tek.Create(tklf));
                    }/*TempData["msg"] = tek.Create(tklf) ? "Teklife mesajınız iletildi.." : "Hata !! Lütfen Tekrar Deneyiniz.."*/
                    return View(tek.Create(tklf));
                }
                //TempData["msg"] = tek.Create(tklf) ? "Teklife mesajınız iletildi.." : "Hata !! Lütfen Tekrar Deneyiniz..";

                return PartialView(tek.Create(tklf));
            }
            else
            {
                return RedirectToAction("PageError", "Error");
            }
        }

        public ActionResult TeklifVer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TeklifVer(Teklifler model)
        {
            if (ModelState.IsValid)
            {
                int tv = ((User)Session["user"]).UserID;
                int use = model.UrunId;
                int z = db.Urunlers.Where(x => x.UrunId == use).Select(x => x.UserID).SingleOrDefault();
                if (z == tv)
                    return View(TempData["msg"] = "Bu urun size ait ..");


                model.TeklifVeren = tv;
                model.TeklifTarihi = DateTime.Now;
                model.Kime = z;

                if (Session["urunid"] == null)
                {
                    var g = db.Urunlers.Where(x => x.UrunId == model.UrunId).SingleOrDefault();
                    int h = g.AltLimit;
                    int k = model.Tutar;
                    if (k < h)
                        return View(TempData["msg"] = "Lütfen Teklifiniz makul olsun.");


                    return View(TempData["msg"] = tek.Create(model) ? "Teklifiniz iletildi.. " : "Hata ! Lütfen tekrar deneyiniz ...");
                }

                else
                {
                    model.UrunId = Convert.ToInt32(Session["urunid"]);
                    urn.Bring(Convert.ToInt32(Session["urunid"]));
                    int d = Convert.ToInt32(Session["urunid"]);
                    var e = db.Urunlers.Where(x => x.UrunId == d).SingleOrDefault();
                    int a = e.AltLimit;
                    int b = model.Tutar;
                    if (b < a)
                        return View(TempData["msg"] = "Lütfen Teklifiniz makul olsun.");
                    
                    else
                        return View(TempData["msg"] = tek.Create(model) ? "Teklifiniz iletildi.. " : "Hata ! Lütfen tekrar deneyiniz ...");
                  
                }
            }
            else if (model.Tutar==0)
            {
                return View(TempData["msg"] = "Lutfen Gecerli bir Tutar giriniz...");
            }
            return View(TempData["msg"] = "Buradan sadece Tutar belirte bilirsiniz...");


        }


        public ActionResult TeklifOku(int id)
        {
            Mesajlar vm = new Mesajlar();
            if (Session["user"] is User kullanici)
            {
                int urunId = db.Tekliflers.Where(x => x.TeklifId == id).Select(z => z.UrunId).SingleOrDefault();
                int
                    gonderenId = db.Tekliflers.Where(v => v.TeklifId == id).Select(b => b.TeklifVeren).SingleOrDefault(),
                    kimeId = db.Tekliflers.Where(v => v.TeklifId == id).Select(b => b.Kime).SingleOrDefault();

                Session["tekurun"] = urunId;
                Session["tekgond"] = gonderenId;

                vm.Kime = kullanici;
                var gonderenler = db.Tekliflers.Where(x => x.UrunId == urunId && x.TeklifVeren == gonderenId && x.Kime == kimeId);
                vm.TeklifIcerik = gonderenler.Concat(db.Tekliflers.Where(x => x.UrunId == urunId && x.TeklifVeren == kimeId && x.Kime == gonderenId)).OrderBy(x => x.TeklifTarihi);
                vm.Urunler = db.Urunlers.Where(x => x.UrunId == urunId).SingleOrDefault();
                vm.Kime = db.Users.Where(x => x.UserID == kimeId).SingleOrDefault();
                if (kullanici.UserID != gonderenId)
                {
                    vm.Kime = db.Users.Where(x => x.UserID == gonderenId).SingleOrDefault();
                    foreach (var item in vm.TeklifIcerik)
                    {
                        int a = item.TeklifId;
                        Teklifler tk = tek.Bring(a);
                        tk.Gorulme = +1;
                        tek.Edit(tk);
                        if (item.TeklifVeren != gonderenId)
                        {
                            vm.TeklifIcerik = gonderenler.Concat(db.Tekliflers.Where(x => x.UrunId == urunId && x.TeklifVeren == kimeId && x.Kime == gonderenId)).OrderByDescending(x => x.TeklifTarihi).Take(1);
                            foreach (var ID in vm.TeklifIcerik)
                            {
                                Teklifler tk1 = tek.Bring(ID.TeklifId);
                                tk1.Gorulme = 0;
                                tek.Edit(tk1);
                            }

                        }
                    }
                }
                Mesajlar son = new Mesajlar();
                son.TeklifIcerik = gonderenler.Concat(db.Tekliflers.Where(x => x.UrunId == urunId && x.TeklifVeren == kimeId && x.Kime == gonderenId)).OrderBy(x => x.TeklifTarihi);
                son.Urunler = db.Urunlers.Where(x => x.UrunId == urunId).SingleOrDefault();
                son.Kime = db.Users.Where(x => x.UserID == kimeId).SingleOrDefault();
                return View(son);
            }

            else
            {
                return RedirectToAction("PageError", "Error");
            }


        }
        public ActionResult MesajYaz(int id)
        {
            Mesajlar vm = new Mesajlar();
            vm.Teklifler = db.Tekliflers.Where(x => x.TeklifId == id).SingleOrDefault();
            int a = db.Tekliflers.Where(x => x.TeklifId == id).Select(x => x.UrunId).SingleOrDefault();
            vm.Mesajs = db.Mesajs.Where(x => x.UrunId == a).ToList();
            return View(vm);
        }

        [HttpPost]
        public ActionResult MesajYaz(Mesaj mesaj)
        {
            int a = mesaj.UrunId;
            mesaj.Gonderen = db.Urunlers.Where(x => x.UrunId == a).Select(x => x.UserID).SingleOrDefault();
            mesaj.MesajTarihi = DateTime.Now;
            return View(TempData["msg"] = msg.Create(mesaj) ? "Mesajınız iletildi.. " : "Hata ! Lütfen tekrar deneyiniz ...");
        }

        public string TeklifSil(int id)
        {
            try
            {
                tek.Delete(id);
                return "1";
            }
            catch (Exception)
            {

                return "-1";
            }
        }

        public string TkfSil(Teklifler tk)
        {
            try
            {
                var ms = db.Tekliflers.Where(x => x.TeklifVeren == tk.TeklifVeren && x.UrunId == tk.UrunId);
                foreach (var item in ms)
                {

                    tek.Delete(item.TeklifId);
                }

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
                tek.Dispose();
                urn.Dispose();
                db.Dispose();
                msg.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
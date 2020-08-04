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
    [Authorize]
    public class MesajController : Controller
    {
        TeklifContext db = new TeklifContext();
        GenericRepository<Mesaj> msjj = new GenericRepository<Mesaj>(new TeklifContext());
        GenericRepository<Urunler> urn = new GenericRepository<Urunler>(new TeklifContext());
        public ActionResult Index()
        {
            if (Session["user"] is User kullanici)
            {
                return View(db.Mesajs.Where(x => x.Kime == kullanici.UserID).OrderByDescending(x => x.MesajTarihi).ToList());
            }
            else
            {
                return RedirectToAction("PageError", "Error");
            }
        }

        public ActionResult MesajYaz(int id)
        {
            var c = urn.Bring(id);
            Session["kimeid"] = c.UserID;
            return PartialView(c);
        }

        [HttpPost]
        public ActionResult MesajYaz(Mesaj msj)
        {
            if (Session["user"] is User kullanici)
            {
                if (ModelState.IsValid)
                {
                    msj.Gonderen = kullanici.UserID;
                    msj.MesajTarihi = DateTime.Now;
                    if (msj.UrunId == 0)
                    {
                        msj.UrunId = Convert.ToInt32(Session["urunid"]);
                        msj.Kime = Convert.ToInt32(Session["kimeid"]);
                        return View(TempData["msg"] = msjj.Create(msj) ? "Mesajınız iletildi.." : "HAta !! Lütfen Tekrar Deneyiniz..");
                    }

                    return View(TempData["msg"] = msjj.Create(msj) ? "Mesajınız iletildi.." : "HAta !! Lütfen Tekrar Deneyiniz..");
                }
                return View();
            }
            else
            {
                return RedirectToAction("PageError", "Error");
            }

        }

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult MsjOku(int id)
        {
            Mesajlar msg = new Mesajlar();
            if (Session["user"] is User kullanici)
            {
                int urunId = db.Mesajs.Where(x => x.MesajId == id).Select(z => z.UrunId).SingleOrDefault();

                int
                    gonderenId = db.Mesajs.Where(v => v.MesajId == id).Select(b => b.Gonderen).SingleOrDefault(),
                    kimeId = db.Mesajs.Where(v => v.MesajId == id).Select(b => b.Kime).SingleOrDefault();
                Session["msjurun"] = urunId;
                Session["msjgond"] = gonderenId;

                var gonderenler = db.Mesajs.Where(x => x.UrunId == urunId && x.Gonderen == gonderenId && x.Kime == kimeId);
                msg.MesajIcerik = gonderenler.Concat(db.Mesajs.Where(x => x.UrunId == urunId && x.Gonderen == kimeId && x.Kime == gonderenId)).OrderBy(x => x.MesajTarihi);
                msg.Urunler = db.Urunlers.Where(x => x.UrunId == urunId).SingleOrDefault();
                msg.Kime = db.Users.Where(x => x.UserID == kimeId).SingleOrDefault();
                if (kullanici.UserID != gonderenId)
                {
                    msg.Kime = db.Users.Where(x => x.UserID == gonderenId).SingleOrDefault();
                    foreach (var item in msg.MesajIcerik)
                    {
                        int mid = item.MesajId;
                        Mesaj mes = msjj.Bring(mid);
                        mes.Okunma = +1;
                        msjj.Edit(mes);

                        if (item.Gonderen != gonderenId)
                        {
                            msg.MesajIcerik = gonderenler.Concat(db.Mesajs.Where(x => x.UrunId == urunId && x.Gonderen == kimeId && x.Kime == gonderenId)).OrderByDescending(x => x.MesajTarihi).Take(1);
                            foreach (var ID in msg.MesajIcerik)
                            {
                                Mesaj mes1 = msjj.Bring(ID.MesajId);
                                mes1.Okunma = 0;
                                msjj.Edit(mes1);
                            }

                        }
                    }
                }
                Mesajlar son = new Mesajlar();
                son.MesajIcerik = gonderenler.Concat(db.Mesajs.Where(x => x.UrunId == urunId && x.Gonderen == kimeId && x.Kime == gonderenId)).OrderBy(x => x.MesajTarihi);
                son.Urunler = db.Urunlers.Where(x => x.UrunId == urunId).SingleOrDefault();
                son.Kime = db.Users.Where(x => x.UserID == kimeId).SingleOrDefault();
                return View(son);
            }
            return RedirectToAction("PageError", "Error");
        }

        public ActionResult MsjYaz(Mesaj msj)
        {
            if (Session["user"] is User kullanici)
            {
                msj.Gonderen = kullanici.UserID;
                msj.MesajTarihi = DateTime.Now;
                if (msj.UrunId == 0)
                {
                    msj.UrunId = Convert.ToInt32(Session["msjurun"]);
                    msj.Kime = Convert.ToInt32(Session["msjgond"]);
                    if (msj.Gonderen == msj.Kime)
                    {
                        msj.Kime = Convert.ToInt32(Session["gonderilecek"]);
                        msjj.Create(msj);
                        return PartialView(msjj.Bring(msj.MesajId));
                    }
                    msjj.Create(msj);
                    return PartialView(msjj.Bring(msj.MesajId));
                }
                msjj.Create(msj);
                return PartialView(msjj.Bring(msj.MesajId));
            }
            else
            {
                return RedirectToAction("PageError", "Error");
            }
        }

        public string MesajSil(int id)
        {
            try
            {
                msjj.Delete(id);
                return "1";
            }
            catch (Exception)
            {

                return "-1";
            }
        }


        public string MsjSil(Mesaj mesaj)
        {

            try
            {

                var ms = db.Mesajs.Where(x => x.Gonderen == mesaj.Gonderen && x.UrunId == mesaj.UrunId);
                foreach (var item in ms)
                {

                    msjj.Delete(item.MesajId);
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
                db.Dispose();
                msjj.Dispose();
                urn.Dispose();

            }
            base.Dispose(disposing);

        }

    }

}
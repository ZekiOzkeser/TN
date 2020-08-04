using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TN.DAL;
using TN.Models;
using TN.Models.Extended;
using PagedList;
using TN.BLL;

namespace TeklifinNedir.Controllers
{
    public class UrunController : Controller
    {

        TeklifContext db = new TeklifContext();
        GenericRepository<Urunler> urn = new GenericRepository<Urunler>(new TeklifContext());


     
        public ActionResult Index(int Page = 1)
        {
            var lst = db.Urunlers.Where(m => m.Yayın == true).OrderByDescending(m => m.UrunId).ToPagedList(Page, 15);

            return View(lst);
        }


        public ActionResult Urun1()
        {
            var d = db.Urunlers.Where(x => x.Yayın == true).ToList();
            return View(d);
        }
        public ActionResult Urun2()
        {
            Random rnd = new Random();
            int id = rnd.Next(1, 10);
            if (db.Resimlers.Where(m => m.Urunlers.KategoriId == id).ToList() != null)
            {
                var d = db.Urunlers.Where(m => m.KategoriId == id && m.Yayın==true).ToList();
                return View(d);
            }

            var d2 = db.Urunlers.Where(m => m.KategoriId == id && m.Yayın==true).ToList();
            return View(d2);
        }
        public ActionResult Urun3()
        {
            Random rnd = new Random();
            int id = rnd.Next(1, 10);
            if (db.Resimlers.Where(m => m.Urunlers.KategoriId == id).ToList() != null)
            {
                var d = db.Urunlers.Where(m => m.KategoriId == id &&m.Yayın==true).ToList();
                return View(d);
            }

            var d2 = db.Urunlers.Where(m => m.KategoriId == id && m.Yayın==true).ToList();
            return View(d2);
        }
        public ActionResult Urun4()
        {
            Random rnd = new Random();
            int id = rnd.Next(1, 10);
            if (db.Resimlers.Where(m => m.Urunlers.KategoriId == id).ToList() != null)
            {
                var d = db.Urunlers.Where(m => m.KategoriId == id && m.Yayın==true).ToList();
                return View(d);
            }

            var d2 = db.Urunlers.Where(m => m.KategoriId == id && m.Yayın==true).ToList();
            return View(d2);
        }

        [Route("Kategoriler/@={id}/{Page=1}")]
        public ActionResult KatGore(int id, int Page = 1)
        {
            var lst = db.Urunlers.Where(m => m.KategoriId == id && m.Yayın==true).OrderByDescending(m => m.UrunId).ToPagedList(Page, 15);
            

            return View(lst);
        }

        public ActionResult EnCokBakilanlar()
        {
            return View(urn.Listele().Where(x => x.Yayın == true).OrderByDescending(z => z.Bakilma).Take(5));
        }


        public ActionResult Kategoriler()
        {

            TumEkranlar lst = new TumEkranlar();
            lst.Kategorilers = db.Kategorilers.ToList();
            lst.Urunlers = db.Urunlers.Where(x => x.Yayın == true).ToList();

            return View(lst);
        }


        [Route("Urunler/Urun/@={id}/Detay")]
        public ActionResult UrunDetay(int id)
        {
            AnaSayfa a = new AnaSayfa();
            a.Resimlers = db.Resimlers.Where(x => x.UrunId == id).ToList();
            a.Urunler = db.Urunlers.Where(x => x.UrunId == id).Single();
            int c = a.Urunler.UserID;
            a.Iletisim = db.Iletisims.Where(x => x.UserID == c).Single();
            int il = a.Iletisim.IlId;
            int ilce = a.Iletisim.ID;
            a.Il = db.Il.Where(x => x.IlId == il).Single();
            a.Ilce = db.Ilce.Where(x => x.ID == ilce).Single();
            Urunler urun = urn.Bring(id);
            urun.Bakilma += 1;
            urn.Edit(urun);
            return View(a);
        }

        public ActionResult Detay(int id)
        {
            return View(db.Resimlers.Where(m => m.UrunId == id).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                urn.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
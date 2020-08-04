using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TN.BLL;
using TN.DAL;
using TN.Models;
using TN.Models.Extended;
using PagedList;

namespace TN.Controllers
{
    [Authorize]
  
    public class KullaniciController : Controller
    {
        TeklifContext db = new TeklifContext();
        GenericRepository<User> us = new GenericRepository<User>(new TeklifContext());
        GenericRepository<Urunler> urn = new GenericRepository<Urunler>(new TeklifContext());
        GenericRepository<Iletisim> ilet = new GenericRepository<Iletisim>(new TeklifContext());

        [Route("ayarlar")]
        public ActionResult Index()
        {
            if (Session["user"] is User kullanici)
            {
                if (kullanici.EmailID == "zekiozkeser@hotmail.com")
                {
                    return RedirectToAction("Index", "Admin");
                }
                ViewModel vm = new ViewModel();
                vm.Kategorilers = db.Kategorilers.OrderByDescending(x => x.KategoriAdi).ToList();
                vm.Urunlers = db.Urunlers.OrderByDescending(x => x.UrunOlusturma).ToList();
                Session["Urunler"] = db.Urunlers.Where(m => m.UserID == kullanici.UserID).ToList().Count;
                Session["mesajlar"] = db.Mesajs.Where(m => m.Kime == kullanici.UserID).ToList().Count;
                Session["teklifler"] = db.Tekliflers.Where(m => m.Urunler.UserID == kullanici.UserID).Count();
                int c = db.Urunlers.Where(m => m.UserID == kullanici.UserID).Select(x => x.Bakilma).Count();
                if (c != 0)
                    Session["bakilma"] = db.Urunlers.Where(m => m.UserID == kullanici.UserID).Sum(x => x.Bakilma);
              
                return View(vm);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [Route("Acikartirma/kapali")]
        public ActionResult AcikArtirma()
        {
            int kalanKullanici = 4000 - db.Users.Count();
            
            List<int> rakamlar = new List<int>();

            Session["rakam1"] = 0;
            Session["rakam2"] = 0;
            Session["rakam3"] = 0;
            Session["rakam4"] = 0;

            if (kalanKullanici > 0)
            {
                int kat = 10;
                for (int i = 4 - 1; i >= 0; i--)
                {
                    rakamlar.Add(kalanKullanici % kat);
                    kalanKullanici -= rakamlar[rakamlar.Count - 1];
                    kalanKullanici /= 10;
                }

                Session["rakam1"] = rakamlar[3];
                Session["rakam2"] = rakamlar[2];
                Session["rakam3"] = rakamlar[1];
                Session["rakam4"] = rakamlar[0];
            }

            return View();
        }

     
     

        public ActionResult HeaderDesktop()
        {
            if (Session["user"] is User kullanici)
            {
                ViewModel vm = new ViewModel();
                vm.Kategorilers = db.Kategorilers.ToList();
                vm.Urunlers = db.Urunlers.Where(m => m.Yayın == true).ToList();
                vm.Users = db.Users.ToList();
                vm.Mesajs = db.Mesajs.Where(x => x.Kime == kullanici.UserID).Where(x => x.Okunma == 0).OrderByDescending(x => x.MesajTarihi).Take(4);
                vm.Tekliflers = db.Tekliflers.Where(x => x.Kime == kullanici.UserID).Where(x => x.Gorulme == 0).OrderByDescending(x => x.TeklifTarihi).Take(5);

                var tek = db.Tekliflers.Where(x => x.Kime == kullanici.UserID && x.Gorulme==0).ToList();
                var mes = db.Mesajs.Where(x => x.Kime == kullanici.UserID && x.Okunma==0).ToList();
                Session["msjsay"] = mes.Where(z => z.Kime==kullanici.UserID).Count(); 
                Session["teksay"] = tek.Where(x=>x.Kime==kullanici.UserID).Count(); 
                return View(vm);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public ActionResult HeaderMobile()
        {
            if (Session["user"] is User kullanici)
            {
                ViewModel vm2 = new ViewModel();
                vm2.Kategorilers = db.Kategorilers.ToList();
                vm2.Urunlers = db.Urunlers.Where(m => m.Yayın == true).ToList();
                vm2.Users = db.Users.ToList();
                vm2.Mesajs = db.Mesajs.Where(x => x.Kime == kullanici.UserID).Where(x => x.Okunma == 0).OrderByDescending(x => x.MesajTarihi).Take(4);
                vm2.Tekliflers = db.Tekliflers.Where(x => x.Kime == kullanici.UserID).Where(x => x.Gorulme == 0).OrderByDescending(x => x.TeklifTarihi).Take(5);
                return View(vm2);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        public ActionResult AltMenu()
        {
            TumEkranlar tum = new TumEkranlar();
            tum.Kategorilers = db.Kategorilers.ToList().Take(6);
            tum.Urunlers = db.Urunlers.Where(m=>m.Yayın==true).ToList().OrderByDescending(x => x.Baslik).Take(5);
            return View(tum);
        }

        [Route("Urunlerimzz")]
        public ActionResult Urunlerim()
        {

            if (Session["user"] is User kullanici)
            {
                return View(db.Urunlers.Where(m => m.UserID == kullanici.UserID).OrderByDescending(x => x.UrunOlusturma).ToList());
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }


        [Route("YayındaOlanUrunlerim")]
        public ActionResult Aktif()
        {
            if (Session["user"] is User kullanici)
            {
                var d = db.Urunlers.Where(m => m.UserID == kullanici.UserID).ToList();
                return View(d.Where(m => m.Yayın == true).OrderByDescending(x => x.UrunOlusturma).ToList());
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }


        [Route("YayındaOlmayanUrunlerim")]
        public ActionResult Pasif()
        {
            if (Session["user"] is User kullanici)
            {
                var d = db.Urunlers.Where(m => m.UserID == kullanici.UserID).ToList();
                return View(d.Where(m => m.Yayın == false).ToList());
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }


        [Route("Bilgilerim")]
        public ActionResult Bilgilerim()
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

                var i = db.Iletisims.Where(x => x.UserID == kullanici.UserID).SingleOrDefault();
                int il = i.IlId;
                int ilce = i.ID;

                UrunKullanici kul = new UrunKullanici
                {
                    User = db.Users.Where(m => m.UserID == kullanici.UserID).SingleOrDefault(),
                    Iletisim = db.Iletisims.Where(m => m.UserID == kullanici.UserID).SingleOrDefault(),
                    Il = db.Il.Where(m => m.IlId == il).SingleOrDefault(),
                    Ilce = db.Ilce.Where(m => m.ID == ilce).SingleOrDefault()
                };

                return View(kul);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

   
        public ActionResult BilgiDuzenle()
        {
            if (Session["user"] is User kullanici)
            {
                return View(kullanici);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

       
        [HttpPost]
        public ActionResult BilgiDuzenle(User user)
        {
            if (Session["user"] is User d)
            {
                user.ActivationCode = d.ActivationCode;
                user.EmailID = d.EmailID;
                user.IsEmailVerified = d.IsEmailVerified;
                user.Password = d.Password;
                user.ResetPasswordCode = d.ResetPasswordCode;
                user.Sozlesme = d.Sozlesme;
                user.UserID = d.UserID;
                user.UyelikBaslangic = d.UyelikBaslangic;

                TempData["msg"] = us.Edit(user) ? "Bilgileriniz Düzenlendi.." : "Hata Tekrar Deneyiniz !!!";
                return RedirectToAction("Index", "Kullanici");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public ActionResult IletisimDuzenle()
        {
            if (Session["user"] is User kullanici)
            {
                var d = db.Iletisims.Where(m => m.UserID == kullanici.UserID).SingleOrDefault();
                return View(d);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }


        [HttpPost]
        public ActionResult IletisimDuzenle(FormCollection c)
        {
            if (Session["user"] is User kullanici)
            {
                if (ModelState.IsValid)
                {
                    Iletisim ileti = new Iletisim();
                    ileti.IlId = Convert.ToInt32(c["il"]);
                    ileti.ID = Convert.ToInt32(c["ilce"]);
                    ileti.Adres = c["Adres"].ToString();
                    ileti.Tel = c["Tel"].ToString();
                    ileti.IletisimId = Convert.ToInt32(c["IletisimId"]);
                    ileti.UserID = kullanici.UserID;
                    TempData["msg"] = ilet.Edit(ileti) ? "İletişim Bilgileriniz Kaydedildi.." : "Hata ! Lütfen Tekrar Deneyiniz...";
                    return RedirectToAction("Index", "Kullanici");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [Route("GelenMesajlarım")]
        public ActionResult Mesajlarim()
        {
            if (Session["user"] is User kullanici)
            {
                Dictionary<int, MesajGrubu> mesajlar = new Dictionary<int, MesajGrubu>();
                var mesajListesi = db.Mesajs.Where(m => m.Gonderen == kullanici.UserID || m.Kime == kullanici.UserID).OrderByDescending(m => m.MesajTarihi).ToArray();
                int okunmayan = 0;

                foreach (var mesaj in mesajListesi)
                {
                    if (!mesajlar.ContainsKey(mesaj.UrunId))
                        mesajlar.Add(mesaj.UrunId, new MesajGrubu() { UrunId = mesaj.UrunId, Kullanici = kullanici.UserID });

                    if (mesaj.Gonderen != kullanici.UserID)
                    {
                        if (mesaj.Okunma == 0)
                            okunmayan++;

                        mesajlar[mesaj.UrunId].Mesajlar.Add(mesaj);
                    }
                    else if (!mesajlar.ContainsKey(mesaj.Kime))
                        mesajlar[mesaj.UrunId].Mesajlar.Add(mesaj);

                    Session["urngorsl"] = db.Urunlers.Where(x => x.UrunId == mesaj.UrunId).Select(c => c.AnaGorsel.Resimler.ImageUrl).SingleOrDefault();
                  

                }

                ViewModel vm = new ViewModel
                {
                    UserID = kullanici.UserID,
                    OkunmayanSayisi = okunmayan,
                    Users = db.Users.ToList(),
                    GelenMesajlar = mesajlar
                };
               
                return View(vm);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [Route("GelenTekliflerim")]
        public ActionResult Tekliflerim()
        {
            if (Session["user"] is User kullanici)
            {
                Dictionary<int, TeklifGrubu> teklifler = new Dictionary<int, TeklifGrubu>();
                var teklifListesi = db.Tekliflers.Where(m => m.TeklifVeren == kullanici.UserID || m.Kime == kullanici.UserID).OrderByDescending(m => m.TeklifTarihi).ToArray();
                int okunmayan = 0;

                foreach (var teklif in teklifListesi)
                {
                    if (!teklifler.ContainsKey(teklif.UrunId))
                        teklifler.Add(teklif.UrunId, new TeklifGrubu() { UrunId = teklif.UrunId, Kullanici = kullanici.UserID });

                    if (teklif.TeklifVeren != kullanici.UserID)
                    {
                        if (teklif.Gorulme == 0)
                            okunmayan++;

                        teklifler[teklif.UrunId].Teklifler.Add(teklif);
                    }
                    else if (!teklifler.ContainsKey(teklif.Kime))
                        teklifler[teklif.UrunId].Teklifler.Add(teklif);

                    Session["urngorsl2"] = db.Urunlers.Where(x => x.UrunId == teklif.UrunId).Select(c => c.AnaGorsel.Resimler.ImageUrl).SingleOrDefault();
                }

                ViewModel vm = new ViewModel
                {
                    UserID = kullanici.UserID,
                    OkunmayanSayisi = okunmayan,
                    Users = db.Users.ToList(),
                    GelenTeklifler = teklifler
                };

                return View(vm);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [Route("Urunler/{Page=1}")]
        public ActionResult UrunIndex(int Page = 1)
        {
            var lst = db.Urunlers.Where(m=>m.Yayın==true).OrderByDescending(m => m.UrunId).ToPagedList(Page, 15);
            return View(lst);
        }

        [Route("Urunler/@{id}/{Page=1}/Kategoriler")]
        public ActionResult KatGore(int id, int Page = 1)
        {
            var lst = db.Urunlers.Where(m => m.KategoriId == id && m.Yayın==true).OrderByDescending(m => m.UrunId).ToPagedList(Page, 15);
            return View(lst);
        }

        public ActionResult EnCokBakilanlar()
        {
            return View(urn.Listele().Where(m => m.Yayın == true).OrderByDescending(z=>z.Bakilma).Take(5));
        }

        public ActionResult Kategoriler()
        {
            var c = db.Kategorilers.Select(x => x.KategoriId).ToList();
            TumEkranlar lst = new TumEkranlar();
            lst.Kategorilers = db.Kategorilers.ToList();
            lst.Urunlers = db.Urunlers.Where(m => m.Yayın == true).ToList();
            return View(lst);
        }

        [Route("Urunler/={id}/Detay")]
        public ActionResult DetayUrun(int id)
        {
            Session["urunid"] = id;
            AnaSayfa a = new AnaSayfa();
            a.Resimlers = db.Resimlers.Where(x => x.UrunId == id).ToList();
            a.Urunler = db.Urunlers.Where(x => x.UrunId == id).SingleOrDefault();
            int c = a.Urunler.UserID;
            Session["kimeid"] = c;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                us.Dispose();
                urn.Dispose();
                ilet.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}
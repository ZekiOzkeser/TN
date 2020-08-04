using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TN
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

        

            routes.MapRoute(
               name: "UrunEkle",
               url: "Urun/UrunEkle",
               defaults: new { controller = "Post", action = "UrunEkle" },
               constraints: new { method = new HttpMethodConstraint("GET", "POST") }
           );

            routes.MapRoute(
            name: "ResimEkle",
            url: "Urun/ResimEkle",
            defaults: new { controller = "Post", action = "ResimYukle" },
            constraints: new { method = new HttpMethodConstraint("GET", "POST") }
        );
            routes.MapRoute(
          name: "VitrinResmi",
          url: "Urun/VitrinResimSec",
          defaults: new { controller = "Post", action = "AnaResim" },
          constraints: new { method = new HttpMethodConstraint("GET", "POST") }
      );
            routes.MapRoute(
          name: "Kontrol",
          url: "Urun/Kontrol",
          defaults: new { controller = "Post", action = "Kontrol" },
          constraints: new { method = new HttpMethodConstraint("GET", "POST") }
      );
            routes.MapRoute(
          name: "UrunDuzenle2",
          url: "Urun/Duzenleme",
          defaults: new { controller = "Post", action = "UrunDuzenle" },
          constraints: new { method = new HttpMethodConstraint("GET", "POST") }
      );
            routes.MapRoute(
           name: "ResimDuzenle2",
           url: "Urun/ResimDuzenleme",
           defaults: new { controller = "Post", action = "ResimDuzenle" },
           constraints: new { method = new HttpMethodConstraint("GET", "POST") }
       );
            routes.MapRoute(
          name: "VitrinResmiD",
          url: "Urun/VitrinResimDegistir",
          defaults: new { controller = "Post", action = "AnaResimDuz" },
          constraints: new { method = new HttpMethodConstraint("GET", "POST") }
      );
            routes.MapRoute(
          name: "SonDuzen",
          url: "Urun/SonDuzen",
          defaults: new { controller = "Post", action = "Kontrol2" },
          constraints: new { method = new HttpMethodConstraint("GET", "POST") }
      );
            routes.MapRoute(
              name: "BilgilerimiDuzenle",
              url: "Bilgilerim/Duzenle",
              defaults: new { controller = "Kullanici", action = "BilgiDuzenle" },
              constraints: new { method = new HttpMethodConstraint("GET", "POST") }
          );
            routes.MapRoute(
              name: "IletisimBilgilerimiDuzenle",
              url: "Bilgilerim/Iletisim/Duzenle",
              defaults: new { controller = "Kullanici", action = "IletisimDuzenle" },
              constraints: new { method = new HttpMethodConstraint("GET", "POST") }
          );


            routes.MapRoute(
              name: "Uye",
              url: "UyeOl",
              defaults: new { controller = "User", action = "Registration" },
              constraints: new { method = new HttpMethodConstraint("GET", "POST") }
          );

            routes.MapRoute(
              name: "Girisi",
              url: "UyeGirisi",
              defaults: new { controller = "User", action = "Login" },
              constraints: new { method = new HttpMethodConstraint("GET", "POST") }
          );

            routes.MapRoute(
              name: "SifreUnuttum",
              url: "SifremiUnuttum",
              defaults: new { controller = "User", action = "ForgotPassword" },
              constraints: new { method = new HttpMethodConstraint("GET", "POST") }
          );

            routes.MapRoute(
              name: "SifreSifirlama",
              url: "SifremiSifirla",
              defaults: new { controller = "User", action = "ResetPassword" },
              constraints: new { method = new HttpMethodConstraint("GET", "POST") }
          );

            routes.MapRoute(
              name: "Mesaj",
              url: "Mesajlarim/Mesaj/@={id}",
              defaults: new { controller = "Mesaj", action = "MsjOku", id=UrlParameter.Optional },
              constraints: new { method = new HttpMethodConstraint("GET", "POST") }
          );

            routes.MapRoute(
             name: "Teklif",
             url: "Tekliflerim/Teklif/@={id}",
             defaults: new { controller = "Teklif", action = "TeklifOku", id = UrlParameter.Optional },
             constraints: new { method = new HttpMethodConstraint("GET", "POST") }
         );


            routes.MapRoute(
                            name: "Default",
                            url: "{controller}/{action}/{id}",
                            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                        );
        }
    }
}

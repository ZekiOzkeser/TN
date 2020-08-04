using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TN.DAL;
using TN.Models;
using System.ComponentModel.DataAnnotations;
using TN.Models.Extended;
using System.Web.Security;
using System.Net.Mail;
using System.Net;
using TN.BLL;

namespace TeklifinNedir.Controllers
{
    public class UserController : Controller
    {
        TeklifContext dc = new TeklifContext();
        GenericRepository<User> dg = new GenericRepository<User>(new TeklifContext());
        GenericRepository<Iletisim> ilet = new GenericRepository<Iletisim>(new TeklifContext());

        [Authorize]
        public ActionResult Index()
        {

            return View();
        }

        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        //Registration POST action 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")]User1 user)
        {
            bool Status = false;
            string message = "";

            if (ModelState.IsValid)
            {


                #region //Email is already Exist 
                var isExist = IsEmailExist(user.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "E-posta zaten var");
                    return View(user);
                }
                #endregion
                #region Generate Activation Code 
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region  Password Hashing 
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); //
                #endregion
                user.IsEmailVerified = false;
                #region Save to Database
                using (TeklifContext dc = new TeklifContext())
                {
                    user.UyelikBaslangic = DateTime.Now;
                    dc.Users.Add(user);
                    dc.SaveChanges();
                    Session["user"] = user;
                    //Send Email to User
                    SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    message = "Kayıt işleminizi yarıladınız :) Hesap etkinleştirme " +
                        " bağlantısı email adresinize gönderildi:" + user.EmailID;
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Geçersiz istek";
            }


            // Model Validation 


            ViewBag.Message = message;
            ViewBag.Status = Status;

            return View(user);
        }
        //Verify Account  
        public ActionResult Iletisim()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Iletisim(FormCollection c)
        {
            if (ModelState.IsValid)
            {
                Iletisim ileti = new Iletisim();
                ileti.IlId = Convert.ToInt32(c["il"]);
                ileti.ID = Convert.ToInt32(c["ilce"]);
                ileti.Adres = c["Adres"].ToString();
                ileti.Tel = c["Tel"].ToString();
                ileti.UserID = ((User)Session["user"]).UserID;
                TempData["msg"] = ilet.Create(ileti) ? "İletişim Bilgileriniz Kaydedildi.." : "Hata ! Lütfen Tekrar Deneyiniz...";
                return RedirectToAction("Index", "Kullanici");

            }

            return View();
        }

        [HttpPost]
        public JsonResult IlIlce(int? ilId, string tip)
        {
            //EntityFramework ile veritabanı modelimizi oluşturduk ve
            //IlilceDBEntities ile db nesnesi oluşturduk.

            //geriye döndüreceğim sonucListim
            List<SelectListItem> sonuc = new List<SelectListItem>();
            //bu işlem başarılı bir şekilde gerçekleşti mi onun kontrolunnü yapıyorum
            bool basariliMi = true;
            try
            {
                switch (tip)
                {
                    case "ilGetir":
                        //veritabanımızdaki iller tablomuzdan illerimizi sonuc değişkenimze atıyoruz
                        foreach (var il in dc.Il.ToList())
                        {
                            sonuc.Add(new SelectListItem
                            {
                                Text = il.Ad,
                                Value = il.IlId.ToString()
                            });
                        }
                        break;
                    case "ilceGetir":
                        //ilcelerimizi getireceğiz ilimizi selecten seçilen ilID sine göre 
                        foreach (var ilce in dc.Ilce.Where(il => il.IlId == ilId).ToList())
                        {
                            sonuc.Add(new SelectListItem
                            {
                                Text = ilce.Ad,
                                Value = ilce.ID.ToString()
                            });
                        }
                        break;

                    default:
                        break;

                }
            }
            catch (Exception)
            {
                //hata ile karşılaşırsak buraya düşüyor
                basariliMi = false;
                sonuc = new List<SelectListItem>();
                sonuc.Add(new SelectListItem
                {
                    Text = "Bir hata oluştu :(",
                    Value = "Default"
                });

            }
            //Oluşturduğum sonucları json olarak geriye gönderiyorum
            return Json(new { ok = basariliMi, text = sonuc });
        }

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (TeklifContext dc = new TeklifContext())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
                var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Geçersiz istek";
                }
            }
            ViewBag.Status = Status;
            return View();

        }

        //Login 
        [HttpGet]
        public ActionResult Login()
        {

            return View();

        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, User u, string ReturnUrl = "")
        {
            string message = "";
            using (TeklifContext dc = new TeklifContext())
            {
                var v = dc.Users.Where(a => a.EmailID == login.EmailID).FirstOrDefault();
                if (v != null)
                {
                    if (!v.IsEmailVerified)
                    {
                        ViewBag.Message = "Lütfen önce email adresinizi doğrulayın..";
                        return View();
                    }

                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        if (login.RememberMe == true)
                        {
                            HttpCookie cerezz = new HttpCookie("cerezim");
                            cerezz.Values.Add("Email", u.EmailID);
                            cerezz.Values.Add("Password", login.Password);
                            cerezz.Expires = DateTime.Now.AddDays(30);
                            Response.Cookies.Add(cerezz);
                        }
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;


                        Response.Cookies.Add(cookie);


                        if (Url.IsLocalUrl(ReturnUrl))
                        {

                            return Redirect(ReturnUrl);
                        }
                        else
                        {

                            var c = dc.Users.Where(p => p.EmailID == u.EmailID).FirstOrDefault();
                            Session["user"] = c;
                            return RedirectToAction("UrunIndex", "Kullanici");
                        }
                    }
                    else
                    {
                        message = "Geçersiz kimlik bilgisi..";
                    }
                }
                else
                {
                    message = "Geçersiz kimlik bilgisi..";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session["user"] = null;
            Session["teksay"] = null;
            Session["msjsay"] = null;
            Session["kimeid"] = null;
            Session["urunid"] = null;
            HttpContext.User = null;
            if (Request.Cookies["cerezim"] != null)
            {
                Response.Cookies["cerezim"].Expires = DateTime.Now.AddDays(-1);
            }

            return RedirectToAction("Index", "Home");
        }

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (TeklifContext dc = new TeklifContext())
            {
                var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/User/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("info@*******.com", "teklifiNedir.com");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "**********!";

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Hesabınız başarıyla oluşturuldu!";
                body = "<br/><br/>TeklifiNedir.com hesabınızı başarıyla oluşturulduğunuzu" +
                    " size söylemekten mutluluk duyarız. Hesabınızı doğrulamak için " +
                    "lütfen aşağıdaki bağlantıya tıklayın" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Şifre Sıfırlama";
                body = "Merhaba,<br/><br/>Hesap şifrenizi sıfırlamak istediniz. Şifrenizi sıfırlamak için lütfen aşağıdaki linke tıklayın.." +
                    "<br/><br/><a href=" + link + ">Şifre sıfırlama linki</a>";
            }


            var smtp = new SmtpClient
            {
                Host = "mail.*******.com",
                Port = 587,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        //Part 3 - Forgot Password

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";
            bool status = false;

            using (TeklifContext dc = new TeklifContext())
            {
                var account = dc.Users.Where(a => a.EmailID == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.EmailID, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    //in our model class in part 1
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    message = "Şifre sıfırlama linki e-mail adresinize yollandı..";
                }
                else
                {
                    message = "Hesap bulunamadı !!";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }


            using (TeklifContext dc = new TeklifContext())
            {
                var user = dc.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (TeklifContext dc = new TeklifContext())
                {
                    var user = dc.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "Yeni şifre başarıyla güncellendi...";
                    }
                }
            }
            else
            {
                message = "Geçersiz şifre...";
            }
            ViewBag.Message = message;
            return View(model);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dc.Dispose();
                dg.Dispose();
                ilet.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}

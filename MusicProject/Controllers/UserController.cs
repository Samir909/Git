using MusicProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MusicProject.Controllers
{
    public class UserController : Controller
    {
        // GET: User

        [HttpGet]
        public ActionResult Regis()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Regis([Bind(Exclude = "IsEmailVerified,ActivationCode")] User user)
        {
            bool Status = false;
            string message = "";
            if (ModelState.IsValid)
            {
                #region Email is already Exist
                var isExist = IsEmailExist(user.Email);

                if (isExist)
                {
                   ModelState.AddModelError("EmailExist", "Eamil already exist");
                    return View(user);
                }
                #endregion

                #region Generate Activation Code

                user.ActivationCode = Guid.NewGuid();

                #endregion

                #region  Password Hashing

                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);

                #endregion

                user.IsEmailVerified = false;

                #region Save data to Database

                using (DbModels dc = new DbModels())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();

                    //Send Email to User for verifaction
                    SendVerificationLinkEmail(user.Email, user.ActivationCode.ToString());
                    message = "Registration successfully done.  " +
                              "Activation link has been sent to your email id :" + user.Email;

                    Status = true;

                }

                #endregion


            }
            else
            {
                message = "Invalid Request";
            }


            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }


      

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;

            using (DbModels dc = new DbModels())
            {
                dc.Configuration.ValidateOnSaveEnabled = false;
                var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                  //  User user = new User();

                  
                    try
                    {
                        v.IsEmailVerified = true;
                        //dc.Entry(User).State = EntityState.Modified;
                        //  dc.Entry(user).State = EntityState.Modified;
                        dc.SaveChanges();
                        Status = true;
                    }
                    catch (DbEntityValidationException e)
                    {

                        Console.WriteLine(e);
                    }

                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }

            ViewBag.Status = Status;
            return View();
        }


        [HttpGet]

        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login,string ReturnUrl)
        {


            using (DbModels dc = new DbModels())
            {

                var v = dc.Users.Where(a => a.Email == login.Email).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeOut = login.RememberMe ? 525600 : 20;
                        var ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, timeOut);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeOut);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Client");
                        }


                    }
                    else
                    {
                        ViewBag.Message = "Invalid Request";
                    }


                }

            }

            string message = "";
            ViewBag.Message = message;
            return View();



        }

        [Authorize]
        [HttpPost]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "ClientHome");
        }



        [NonAction]
        public bool IsEmailExist(string Email)
        {
            using (DbModels dc = new DbModels())
            {
                var v = dc.Users.Where(a => a.Email == Email).FirstOrDefault();
                return v != null;
            }


        }
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("******************", "One Music");// Replace with your actual mail 
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "*******************";// Replace with your actual mail password
            string subject = "Your account is successfully created";

            string body = "<br/><br/>We are excited to tell you that your One Music account is" +
                          "successfully created. Please click on the below link to verify your account" +
                          "<br/><br/><a href='" + link + "'>" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
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




    }
}
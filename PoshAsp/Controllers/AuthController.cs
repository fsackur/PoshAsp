using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoshAsp.Models;
using System.Security.Principal;
using System.Web.Security;

namespace PoshAsp.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet] public ActionResult Login()
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                HttpCookie TokenCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(TokenCookie.Value);

                try
                {
                    AuthToken Token = new AuthToken(ticket.UserData);
                    Token.Invalidate();
                    Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost] public ActionResult Login(string Username, string Password, string ReturnUrl)
        {
            try
            {
                AuthToken Token = new AuthToken(Username, Password);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, Username, DateTime.Now, Token.ValidUntil, true, Token.Id.ToString());
                HttpCookie TokenCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                Response.Cookies.Add(TokenCookie);

                if (ReturnUrl == null)
                {
                    ReturnUrl = "/";
                }

                return Redirect(ReturnUrl);
            }
            catch
            {
                return View();
            }
        }
    }
}
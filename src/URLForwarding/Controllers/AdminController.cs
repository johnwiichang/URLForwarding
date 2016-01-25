using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using URLForwarding.Models;
using System.Linq;
using System;
using URLForwarding.Services;
using Microsoft.Extensions.OptionsModel;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;

namespace URLForwarding.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private AppSettings _appsetting;

        private UrlForwardingDbContext entity;

        public AdminController(IOptions<AppSettings> config, UrlForwardingDbContext context)
        {
            _appsetting = config.Value;
            entity = context;
        }

        public IActionResult Index(int? p)
        {
            var pnum = p ?? 1;
            var Urls = entity.Urls.OrderByDescending(x => x.CreateTime).Skip(10 * (pnum - 1)).Take(10);
            ViewBag.PageCount = entity.Urls.Count();
            var PageCount = entity.Urls.Count() / 15 + (entity.Urls.Count() % 15 == 0 ? 0 : 1);
            pnum = pnum > PageCount ? PageCount : (pnum < 1 ? 1 : pnum);
            ViewBag.Page = pnum;
            ViewBag.PageCount = PageCount;
            return View(Urls);
        }

        [HttpGet]
        public IActionResult New()
        {
            return View("Edit", new UrlRecord());
        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {
            var url = entity.Urls.Single(x => x.Short == Id);
            return View(url);
        }

        [HttpPost]
        public IActionResult SaveChanges(UrlRecord url)
        {
            if (url.Short == null)
            {
                url.Short = String.Join("", Guid.NewGuid().ToString().Replace("-", "").Take(5));
                url.CreateTime = DateTime.Now;
                while (true)
                {
                    try
                    {
                        entity.Urls.Add(url);
                        entity.SaveChanges();
                        break;
                    }
                    catch (Exception)
                    {
                        url.Short = String.Join("", Guid.NewGuid().ToString().Replace("-", "").Take(5));
                        continue;
                    }
                }
            }
            else
            {
                try
                {
                    var u = entity.Urls.Single(x => x.Short == url.Short);
                    u.UrlData = url.UrlData;
                    u.IsEnable = url.IsEnable;
                    entity.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Content("<script>alert('Your information insufficient.');location.href='" + Url.Action("Edit", "Admin", new { Id = url.Short }) + "'</script>");
                }
            }
            return new RedirectResult(Url.Action("Index", "Admin", new { }));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(string _password)
        {
            var authprop = new Microsoft.AspNet.Http.Authentication.AuthenticationProperties();
            if (_password == _appsetting.Password)
            {
                authprop.ExpiresUtc = DateTime.UtcNow.AddMinutes(_appsetting.CookieTime);
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("Name", _appsetting.UserName, ClaimValueTypes.String));
                claims.Add(new Claim(ClaimTypes.Name, _appsetting.UserName, ClaimValueTypes.String));
                ClaimsIdentity identity = new ClaimsIdentity(claims, "AuthenticationType", "Name", ClaimTypes.Role);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authprop);
                return RedirectToAction("Index", "Admin");
            }
            return View("CalmDown");
        }

        public async Task<IActionResult> Logoff()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(this.Login), "Admin");
        }
    }
}

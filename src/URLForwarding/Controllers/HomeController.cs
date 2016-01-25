using Microsoft.AspNet.Mvc;
using URLForwarding.Models;
using System.Linq;
using System;

namespace URLForwarding.Controllers
{
    public class HomeController : Controller
    {
        private UrlForwardingDbContext entity;

        public HomeController(UrlForwardingDbContext context)
        {
            entity = context;
        }

        public IActionResult Index(string go)
        {
            try
            {
                var UrlRecord = entity.Urls.Single(x => x.Short == go);
                if (!UrlRecord.IsEnable)
                {
                    return Content("");
                }
                UrlRecord.Counter++;
                entity.SaveChanges();
                return new RedirectResult(UrlRecord.UrlData);
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsRUs2.Models;
using NewsRUs2.Services;

namespace NewsRUs2.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private readonly NewsService _neewSvc;

        public NewsController(NewsService newsService)
        {
            _neewSvc = newsService;
        }

        [AllowAnonymous]
        public ActionResult<IList<News>> Index() => View(_neewSvc.Read());

        [HttpGet]
        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult<News> Create(News neww)
        {
            neww.Created = neww.LastUpdated = DateTime.Now;
            neww.UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            neww.UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                _neewSvc.Create(neww);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult<News> Edit(string id) =>
            View(_neewSvc.Find(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News neww)
        {
            neww.LastUpdated = DateTime.Now;
            neww.Created = neww.Created.ToLocalTime();
            if (ModelState.IsValid)
            {
                if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value != neww.UserId)
                {
                    return Unauthorized();
                }
                _neewSvc.Update(neww);
                return RedirectToAction("Index");
            }
            return View(neww);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            _neewSvc.Delete(id);
            return RedirectToAction("Index");
        }
    }
}

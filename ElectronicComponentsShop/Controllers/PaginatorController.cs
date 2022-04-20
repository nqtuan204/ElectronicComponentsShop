using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Models;

namespace ElectronicComponentsShop.Controllers
{
    public class PaginatorController : Controller
    {
        public ActionResult GetPaginatorPartial(NoNavPaginatorVM paginator)
        {
            return PartialView("_NoNavPaginator", paginator);
        }
        // GET: PaginatorController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PaginatorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PaginatorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaginatorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PaginatorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PaginatorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PaginatorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PaginatorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnessApp.Controllers
{
    public class AthleteController : Controller
    {
        // GET: Athlete
        public ActionResult Index()
        {
            return View();
        }

        // GET: Athlete/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Athlete/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Athlete/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Athlete/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Athlete/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Athlete/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Athlete/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

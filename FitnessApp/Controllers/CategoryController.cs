//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Net.Http;
//using System.Diagnostics;
//using FitnessApp.Models;
//using System.Web.Script.Serialization;

//namespace FitnessApp.Controllers
//{
//    public class CategoryController : Controller
//    {

//        private static readonly HttpClient client;
//        private JavaScriptSerializer jss = new JavaScriptSerializer();

//        static CategoryController()
//        {
//            client = new HttpClient();
//            client.BaseAddress = new Uri("https://localhost:44376/api/categoriesdata/");
//        }

//        // GET: Category/List
//        public ActionResult Index()
//        {
//            //objective: communicate with our category data api to retrieve a list of categories
//            //curl https://localhost:44376/api/categoriesdata/listcategories

//            string url = "listcategories";
//            HttpResponseMessage response = client.GetAsync(url).Result;

//            IEnumerable<CategoryDto> Categories = response.Content.ReadAsAsync<IEnumerable<CategoriesDto>>().Result;

//            return View(Categories);
//        }

//        // GET: Category/Details/5
//        public ActionResult Details(int id)
//        {
//            return View();
//        }

//        // GET: Category/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Category/Create
//        [HttpPost]
//        public ActionResult Create(FormCollection collection)
//        {
//            try
//            {
//                // TODO: Add insert logic here

//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                return View();
//            }
//        }

//        // GET: Category/Edit/5
//        public ActionResult Edit(int id)
//        {
//            return View();
//        }

//        // POST: Category/Edit/5
//        [HttpPost]
//        public ActionResult Edit(int id, FormCollection collection)
//        {
//            try
//            {
//                // TODO: Add update logic here

//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                return View();
//            }
//        }

//        // GET: Category/Delete/5
//        public ActionResult Delete(int id)
//        {
//            return View();
//        }

//        // POST: Category/Delete/5
//        [HttpPost]
//        public ActionResult Delete(int id, FormCollection collection)
//        {
//            try
//            {
//                // TODO: Add delete logic here

//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                return View();
//            }
//        }
//    }
//}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using FitnessApp.Models;
using FitnessApp.Models.ViewModels;
using System.Web.Script.Serialization;

namespace FitnessApp.Controllers
{
    public class CategoryController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CategoryController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44376/api/");
        }

        // GET: Category/List
        public ActionResult Index()
        {
            //objective: communicate with our category data api to retrieve a list of categories
            //curl https://localhost:44376/api/categoriesdata/categorylist

            string url = "categoriesdata/categorylist";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CategoriesDto> Categories = response.Content.ReadAsAsync<IEnumerable<CategoriesDto>>().Result;

            return View(Categories);
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our category data api to retrieve one category
            //curl https://localhost:44376/api/categoriesdata/findcategory/{id}

            CategoryDetails ViewModel = new CategoryDetails();

            string url = "categoriesdata/findcategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CategoriesDto selectedCategory = response.Content.ReadAsAsync<CategoriesDto>().Result;
            ViewModel.SelectedCategory = selectedCategory;

            //showcase info about workout related to this category

            return View(selectedCategory);
        }

       public ActionResult Error()
        {
            return View();
        }

        // GET: Category/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category Category)
        {
            //objective: add a new category into the system using API
            //curl -H "Content-Type: application/json" -d @Category.json https://localhost:44376/api/categoriesdata/addcategory
            string url = "categorydata/addcategory";

            string jsonpayload = jss.Serialize(Category);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            } else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "categoriesdata/findcategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CategoriesDto selectedCategory = response.Content.ReadAsAsync<CategoriesDto>().Result;
            
            return View(selectedCategory);
        }

        // POST: Category/Update/5
        [HttpPost]
        public ActionResult Update(int id, Category Category)
        {

            //objective: communicate with our category data api to retrieve one category
            //curl https://localhost:44376/api/categoriesdata/updatecategory/{id}

            CategoryDetails ViewModel = new CategoryDetails();

            string url = "categoriesdata/updatecategory/" + id;
            string jsonpayload = jss.Serialize(Category);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Category/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "categoriesdata/findcategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CategoriesDto selectedCategory = response.Content.ReadAsAsync<CategoriesDto>().Result;

            return View(selectedCategory);
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //objective: delete the category through API using their categoryID
            ///curl -H "Content-Type: application/json" -d @Category.json https://localhost:44376/api/categoriesdata/deletecategory/{id}

            CategoryDetails ViewModel = new CategoryDetails();

            string url = "categoriesdata/deletecategory/" + id;

            HttpContent content = new StringContent("");

            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Http.Description;
//using FitnessApp.Models;
//using System.Diagnostics;

//namespace FitnessApp.Controllers
//{
//    public class CategoryDataController : ApiController
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        /// <summary>
//        /// Returns all categories in the system
//        /// </summary>
//        /// <returns>
//        /// header: 200 (Ok)
//        /// Content: all categories in the dbase
//        /// </returns>
//        /// <example> 
//        /// GET: api/CategoryData/ListCategories
//        /// </example>

//        [HttpGet]
//        [ResponseType(typeof(CategoriesDto))]
//        public HttpActionResult ListCategories()
//        {
//            return db.Categories;
//        }

//        // GET: api/CategoryData/FindCategory/5
//        [ResponseType(typeof(Category))]
//        public IHttpActionResult FindCategory(int id)
//        {
//            List<Category> Categories = db.Categories.ToList();
//            List<CategoryDto> CategoryDtos = new List<CategoryDto>();

//            Category.ForEach(c => CategoryDtos.Add(new CategoryDto()
//            {
//                CategoryId = c.CategoryId,
//                CategoryName = c.CategoryName
//            }));

//            return Ok(CategoryDtos);
//        }

//        // POST: api/CategoryData/UpdateCategory/5
//        [ResponseType(typeof(void))]
//        [HttpPost]
//        public IHttpActionResult UpdateCategory(int id, Category category)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            if (id != category.CategoryId)
//            {
//                return BadRequest();
//            }

//            db.Entry(category).State = EntityState.Modified;

//            try
//            {
//                db.SaveChanges();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!CategoryExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return StatusCode(HttpStatusCode.NoContent);
//        }

//        // POST: api/CategoryData/AddCategory
//        [HttpPost]
//        [ResponseType(typeof(Category))]
//        public IHttpActionResult PostCategory(Category category)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            db.Categories.Add(category);
//            db.SaveChanges();

//            return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
//        }

//        // POST: DELETE: api/CategoryData/DeleteCategory/5
//        [ResponseType(typeof(Category))]
//        [HttpPost]
//        public IHttpActionResult DeleteCategory(int id)
//        {
//            Category category = db.Categories.Find(id);
//            if (category == null)
//            {
//                return NotFound();
//            }

//            db.Categories.Remove(category);
//            db.SaveChanges();

//            return Ok(category);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        private bool CategoryExists(int id)
//        {
//            return db.Categories.Count(e => e.CategoryId == id) > 0;
//        }
//    }
//}
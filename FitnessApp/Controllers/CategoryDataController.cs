using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using FitnessApp.Models;
using System.Diagnostics;

namespace FitnessApp.Controllers
{
    public class CategoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all categories in the system
        /// </summary>
        /// <returns>
        /// header: 200 (Ok)
        /// Content: all categories in the dbase
        /// </returns>
        /// <example> 
        /// GET: api/CategoryData/categorylist
        /// </example>

        [HttpGet]
        [ResponseType(typeof(CategoriesDto))]
        public IHttpActionResult ListCategory()
        {
            List<Category> Category = db.Categories.ToList();
            List<CategoriesDto> CategoryDtos = new List<CategoriesDto>();

            Category.ForEach(c => CategoryDtos.Add(new CategoriesDto()
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            }));

            return Ok(CategoryDtos);
        }


        /// <summary>
        /// returns all category in the database
        /// </summary>
        /// <param name="id">Category ID primary Key</param>
        /// <returns>
        /// Header: 200(ok)
        /// Content: Categories in the system with their synonymous primary key of Category ID
        /// or
        /// Header: 404 (Not Found)
        /// </returns>
        // GET: api/CategoryData/FindCategory/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult FindCategory(int id)
        {
            Category Category = db.Categories.Find(id);
            CategoriesDto CategoriesDtos = new CategoriesDto()
            {
                CategoryId = Category.CategoryId,
                CategoryName = Category.CategoryName
            };
            if (Category == null)
            {
                return NotFound();
            }

            return Ok(CategoriesDto);
        }

        // POST: api/CategoryData/UpdateCategory/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CategoryData/AddCategory
        [HttpPost]
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
        }

        // POST: DELETE: api/CategoryData/DeleteCategory/5
        [ResponseType(typeof(Category))]
        [HttpPost]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryId == id) > 0;
        }
    }
}
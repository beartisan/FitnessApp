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
        [ResponseType(typeof(CategoriesDto))]
        [HttpGet]
        public IHttpActionResult FindCategory(int id)
        {
            Category Category = db.Categories.Find(id);
            CategoriesDto CategoriesDto = new CategoriesDto()
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

        /// <summary>
        /// Updates specific category in the system with POST Data input
        /// </summary>
        /// <param name="id">Primary Key of Category Id</param>
        /// <param name="category">JSON Data of Category</param>
        /// <returns>
        /// Header: 204 (success)
        /// or
        /// Header: 400 (Bad Request)
        /// or
        /// Header: 404 (Request Not Found)
        /// </returns>
        /// POST: api/CategoryData/UpdateCategory/5
        /// FORM DATA: Category JSON Object
        
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCategory(int id, Category Category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Category.CategoryId)
            {
                return BadRequest();
            }

            db.Entry(Category).State = EntityState.Modified;

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

        /// <summary>
        /// Adds a category to the system
        /// </summary>
        /// <param name="category">JSON Form Data of Category</param>
        /// <returns>
        /// Header: 201 (created)
        /// Content: Category Id, Category Name
        /// or
        /// Header: 400 (Bad Request)
        /// </returns>
        // POST: api/CategoryData/AddCategory
        [HttpPost]
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory(Category Category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(Category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Category.CategoryId }, Category);
        }

        /// <summary>
        /// Adds a category to the system
        /// </summary>
        /// <param name="category">JSON Form Data of Category</param>
        /// <returns>
        /// Header: 201 (created)
        /// Content: Category Id, Category Name
        /// or
        /// Header: 400 (Bad Request)
        /// </returns>     
        /// POST: DELETE: api/CategoryData/DeleteCategory/5
        /// Form Data: Empty
        
        [ResponseType(typeof(Category))]
        [HttpPost]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category Category = db.Categories.Find(id);
            if (Category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(Category);
            db.SaveChanges();

            return Ok();
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
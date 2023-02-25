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

namespace FitnessApp.Controllers
{
    public class AthletesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AthletesData
        public IQueryable<Athlete> GetAthletes()
        {
            return db.Athletes;
        }

        // GET: api/AthletesData/5
        [ResponseType(typeof(Athlete))]
        public IHttpActionResult GetAthlete(int id)
        {
            Athlete athlete = db.Athletes.Find(id);
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        // PUT: api/AthletesData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAthlete(int id, Athlete athlete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != athlete.AthleteId)
            {
                return BadRequest();
            }

            db.Entry(athlete).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AthleteExists(id))
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

        // POST: api/AthletesData
        [ResponseType(typeof(Athlete))]
        public IHttpActionResult PostAthlete(Athlete athlete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Athletes.Add(athlete);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = athlete.AthleteId }, athlete);
        }

        // DELETE: api/AthletesData/5
        [ResponseType(typeof(Athlete))]
        public IHttpActionResult DeleteAthlete(int id)
        {
            Athlete athlete = db.Athletes.Find(id);
            if (athlete == null)
            {
                return NotFound();
            }

            db.Athletes.Remove(athlete);
            db.SaveChanges();

            return Ok(athlete);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AthleteExists(int id)
        {
            return db.Athletes.Count(e => e.AthleteId == id) > 0;
        }
    }
}
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
    public class AthleteDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AthleteData
        [HttpGet]
        [ResponseType(typeof(AthleteDto))]
        public IHttpActionResult ListAthletes()
        {
            List<Athlete> Athletes = db.Athletes.ToList();
            List<AthleteDto> AthleteDtos = new List<AthleteDto>();

            Athletes.ForEach(a => AthleteDtos.Add(new AthleteDto()));
            {
                AthleteId = a.athleteId,
                AthleteFirstName = a.AthleteFirstName,
                AthleteLastName = a.AtheleteLastName
            }

            return Ok(AthleteDtos);
        }

        // GET: api/AthleteData/FindAthelete/5
        [ResponseType(typeof(Athlete))]
        public IHttpActionResult FindAthlete(int id)
        {
            Athlete athlete = db.Athletes.Find(id);
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        // POST: api/AthleteData/UpdateAthlete/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAthlete(int id, Athlete athlete)
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

        // POST: api/AthleteData/AddAthlete
        [ResponseType(typeof(Athlete))]
        [HttpPost]
        public IHttpActionResult AddAthlete(Athlete athlete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Athletes.Add(athlete);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = athlete.AthleteId }, athlete);
        }

        // DELETE: api/AthleteData/DeleteAthlete/5
        [ResponseType(typeof(Athlete))]
        [HttpPost]
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
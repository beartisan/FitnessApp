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
using System.Diagnostics.Eventing.Reader;

namespace FitnessApp.Controllers
{
    public class AthleteDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all athletes in the system
        /// </summary>
        /// <returns>
        /// All athletes in the database including their workout category
        /// </returns>
        /// <example>
        /// GET: api/AthleteData/ListAthletes
        ///</example>

        [HttpGet]
        [ResponseType(typeof(AthleteDto))]
        public IHttpActionResult ListAthletes()
        {
            List<Athlete> Athletes = db.Athletes.ToList();
            List<AthleteDto> AthleteDtos = new List<AthleteDto>();

            Athletes.ForEach(a => AthleteDtos.Add(new AthleteDto()
            {
                AthleteId = a.AthleteId,
                AthleteFirstName = a.AthleteFirstName,
                AthleteLastName = a.AthleteLastName
            }));

            return Ok(AthleteDtos);
        }

        /// <summary>
        /// returns all athletes with a particular workout
        /// </summary>
        /// <return>
        /// all athletes in the database that belongs to a particular workout
        /// </return>
        /// <param name="id">Workout Primary Key</param>
        /// <example>
        /// GET: api/AthleteData/ListAthletesWithThisWorkout/2
        /// </example>
        [HttpGet]
        [ResponseType(typeof(AthleteDto))]
        public IHttpActionResult ListAthletesWithThisWorkout(int id)
        {
            List<Athlete> Athletes = db.Athletes.Where(
                a => a.Workouts.Any(
                    w => w.WorkoutId == id)
                ).ToList();
            List<AthleteDto> AthleteDtos = new List<AthleteDto>();

            Athletes.ForEach(a => AthleteDtos.Add(new AthleteDto()
            {
                AthleteId = a.AthleteId,
                AthleteFirstName = a.AthleteFirstName,
                AthleteLastName = a.AthleteLastName
            }));

            return Ok(AthleteDtos);
        }

        /// <summary>
        /// returns all athletes in the system not associated to a particular workout
        /// </summary>
        /// <param name="id">Primary key of Workout ID</param>
        /// <returns>
        /// content: Shows all athletes in the system NOT associated to a particular workout
        /// </returns>
        ///<example>
        /// GET: api/AthleteData/ListAthletesWithoutThisWorkout/2
        ///</example>
        [ResponseType(typeof(AthleteDto))]
        [HttpGet]
        public IHttpActionResult ListAthletesWithoutThisWorkout(int id)
        {
            List<Athlete> Athletes = db.Athletes.Where(
            a => !a.Workouts.Any(
                w => w.WorkoutId == id)
            ).ToList();
            List<AthleteDto> AthleteDtos = new List<AthleteDto>();

            Athletes.ForEach(a => AthleteDtos.Add(new AthleteDto()
            {
                AthleteId = a.AthleteId,
                AthleteFirstName = a.AthleteFirstName,
                AthleteLastName = a.AthleteLastName
            }));

            return Ok(AthleteDtos);
        }

        /// <summary>
        /// Returns all athletes in the system
        /// </summary>
        /// <returns>
        /// Header: 200(Ok)
        /// Content:
        /// Shows an athlete that belongs with their primary key of AthleteID
        /// or
        /// Header: 404 (Not Found)
        /// </returns>
        /// <param name="id">Primary key of Athlete ID</param>
        /// <example>
        /// GET: api/AthleteData/FindAthlete/2
        /// </example>
        [ResponseType(typeof(AthleteDto))]
        [HttpGet]
        public IHttpActionResult FindAthlete(int id)
        {
            Athlete Athlete = db.Athletes.Find(id);
            AthleteDto AthleteDto = new AthleteDto()
            {
                AthleteId = Athlete.AthleteId,
                AthleteFirstName = Athlete.AthleteFirstName,
                AthleteLastName = Athlete.AthleteLastName
            };
            if (Athlete == null)
            {
                return NotFound();
            }

            return Ok(AthleteDto);
        }

        /// <summary>
        /// Updates a particular athlete in the system using POST data input
        /// </summary>
        /// <returns>
        /// Header: 200(Ok)
        /// or
        /// Header: 400 (Bad Request)
        /// or
        /// Header: 404 (Not Found)
        /// </returns>
        /// <param name="id">Represents the primary key of Athlete ID</param>
        /// <param name="athlete">JSON Form data of an athlete</param>
        /// <example>
        /// GET: api/AthleteData/UpdateAthlete/2
        /// </example>
        [ResponseType(typeof(void))]
        [HttpGet]
        public IHttpActionResult UpdateAthlete(int id, Athlete Athlete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(id != Athlete.AthleteId)
            {
                return BadRequest();
            }
            db.Entry(Athlete).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!AthleteExists(id))
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
        /// Adds an athlete into the system
        /// </summary>
        /// <returns>
        /// Header: 201(Created)
        /// Content: AthleteID, AthleteFirstName, AthleteLastName
        /// or
        /// Header: 400 (Bad Request)
        /// </returns>
        /// <param name="athlete">JSON form data of an Athlete </param>
        /// <example>
        /// POST: api/AthleteData/AddAthlete
        /// FORM DATA: Athlete JSON Object
        /// </example>
        [ResponseType(typeof(Athlete))]
        [HttpPost]
        public IHttpActionResult AddAthlete(Athlete Athlete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //show this
            db.Athletes.Add(Athlete);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Athlete.AthleteId }, Athlete);
        }

        /// <summary>
        /// Removes an athlete into the system through their athleteID
        /// </summary>
        /// <returns>
        /// Header: 200(Ok)
        /// or
        /// Header: 404 (Not Found)
        /// </returns>
        /// <param name="id">Primary Key of the Athlete </param>
        /// <example>
        /// POST: api/AthleteData/DeleteAthlete/2
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Athlete))]
        [HttpPost]
        public IHttpActionResult DeleteAthlete(int id)
        {
            Athlete Athlete = db.Athletes.Find(id);
            if (Athlete == null)
            {
                return NotFound();
            }

            //show this
            db.Athletes.Remove(Athlete);
            db.SaveChanges();

            return Ok();
        }

        //protect
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
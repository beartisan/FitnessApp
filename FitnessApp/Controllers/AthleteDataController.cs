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
        /// content: Shows all athletes in the system not associated to a particular workout
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
        /// GET: api/AthleteData/FindAthlete/2
        /// </example>
        [ResponseType(typeof(AthleteDto))]
        [HttpGet]

    }
}
﻿using System;
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
    public class WorkoutDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all workout in the system.
        /// </summary>
        /// <returns>
        /// CONTENT: all workout in the database, including their categories.
        /// </returns>
        /// <example>
        ///GET: api/WorkoutData/WorkoutList
        /// </example>
       
        [HttpGet]
        public IEnumerable<WorkoutDto> WorkoutList()
        {
            List<Workout> Workouts = db.Workouts.ToList();
            List<WorkoutDto> WorkoutDtos = new List<WorkoutDto>();

            Workouts.ForEach(w => WorkoutDtos.Add(new WorkoutDto()
            {
                WorkoutId = w.WorkoutId,
                WorkoutName = w.WorkoutName,
                WorkoutDate = w.WorkoutDate,
                WorkoutDuration = w.WorkoutDuration,
                CategoryName = w.Category.CategoryName
            }));

            return WorkoutDtos;
        }
        /// <summary>
        /// Provides workout information in the system related to category id
        /// </summary>
        /// <returns>
        /// CONTENT: all workout in the database, including their categories.
        /// </returns>
        /// <param name="id">workout Id
        /// </param>
        /// <example>
        /// GET: api/WorkoutData/FindWorkout/5
        /// </example>
        [ResponseType(typeof(Workout))]
        [HttpGet]
        public IHttpActionResult FindWorkout(int id)
        {
            Workout Workout = db.Workouts.Find(id);
            WorkoutDto WorkoutDto = new WorkoutDto()
            {
                WorkoutId = Workout.WorkoutId,
                WorkoutName = Workout.WorkoutName,
                WorkoutDate = Workout.WorkoutDate,
                WorkoutDuration = Workout.WorkoutDuration,
                CategoryName = Workout.Category.CategoryName
            };

            if (Workout == null)
            {
                return NotFound();
            }

            return Ok(Workout);
        }

        // POST: api/WorkoutData/UpdateWorkout/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateWorkout(int id, Workout Workout)
        {
            Debug.WriteLine("Update Workout Reached");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Invalid Model State");
                return BadRequest(ModelState);
            }

            if (id != Workout.WorkoutId)
            {
                Debug.WriteLine("Unmatched WorkoutId");
                Debug.WriteLine("GET Paramater " +id);
                Debug.WriteLine("POST Paramater " + Workout.WorkoutId);
                Debug.WriteLine("POST Paramater " + Workout.WorkoutName);
                Debug.WriteLine("POST Paramater " + Workout.WorkoutDuration);
                Debug.WriteLine("POST Paramater " + Workout.WorkoutDate);
                return BadRequest();
            }

            db.Entry(Workout).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutExists(id))
                {
                    Debug.WriteLine("Workout not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("No trigger or errors");
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/WorkoutData/AddWorkout
        [ResponseType(typeof(Workout))]
        [HttpPost]
        public IHttpActionResult AddWorkout(Workout Workout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Workouts.Add(Workout);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Workout.WorkoutId }, Workout);
        }

        //POST
        // DELETE: api/WorkoutData/DeleteWorkout/5
        [ResponseType(typeof(Workout))]
        [HttpPost]
        public IHttpActionResult DeleteWorkout(int id)
        {
            Workout Workout = db.Workouts.Find(id);
            if (Workout == null)
            {
                return NotFound();
            }

            db.Workouts.Remove(Workout);
            db.SaveChanges();

            return Ok(Workout);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WorkoutExists(int id)
        {
            return db.Workouts.Count(e => e.WorkoutId == id) > 0;
        }
    }
}
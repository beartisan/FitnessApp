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
        [ResponseType(typeof(WorkoutDto))]
        public IHttpActionResult WorkoutList()
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

            return Ok(WorkoutDtos);
        }

        /// <summary>
        /// Returns information about all workout relating to particular category Id
        /// </summary>
        /// <returns>
        /// CONTENT: all workout in the database that including their associated category Id.
        /// </returns>
        /// <param name="id">Category ID</param>
        /// <example>
        ///GET: api/WorkoutData/WorkoutListForCategory/5
        /// </example>

        [HttpGet]
        [ResponseType(typeof(WorkoutDto))]
        public IHttpActionResult WorkoutListForCategory(int id)
        {
            List<Workout> Workouts = db.Workouts.Where(w=>w.CategoryId==id).ToList();
            List<WorkoutDto> WorkoutDtos = new List<WorkoutDto>();

            Workouts.ForEach(w => WorkoutDtos.Add(new WorkoutDto()
            {
                WorkoutId = w.WorkoutId,
                WorkoutName = w.WorkoutName,
                WorkoutDate = w.WorkoutDate,
                WorkoutDuration = w.WorkoutDuration,
                CategoryId = w.Category.CategoryId,
                CategoryName = w.Category.CategoryName
            }));

            return Ok(WorkoutDtos);
        }

        /// <summary>
        /// Provides workout information in the system
        /// </summary>
        /// <returns>
        /// CONTENT: all workout in the database, matching their workoutID
        /// </returns>
        /// <param name="id">workout Id</param>
        /// <example>
        /// GET: https://localhost:44376/api/WorkoutData/FindWorkout/5
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
                CategoryId = Workout.CategoryId,
                CategoryName = Workout.Category.CategoryName //,
 //               AthletesId = Workout.Athletes.AthletesId
            };

            if (Workout == null)
            {
                return NotFound();
            }

            return Ok(WorkoutDto);
        }

        /// <summary>
        /// updates a specific workout in the system through POST request input
        /// </summary>
        /// <param name="id">synonymous to the primary key of workoutId</param>
        /// <param name="Workout">JSON data format</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/WorkoutData/UpdateWorkout/5
        /// In the terminal ::
        /// curl -d @workout.json -H "content-type: application/json" "https://localhost:44376/api/workoutdata/updateworkout/5"
        /// curl -d https://localhost:44376/api/workoutdata/updateworkout/5
        /// Form Data: Workout JSON Object
        /// </example> 

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

        /// <summary>
        /// adds a workout to the database
        /// </summary>
        /// <param name="Workout">JSON format of workout</param>
        /// <returns>
        /// WorkoutId, WorkoutName, WorkoutDate, WorkoutDuration, CategoryId, CategoryName
        /// </returns>
        /// Form Data: Workout JSON Object
        /// POST: api/WorkoutData/AddWorkout
        
        [ResponseType(typeof(Workout))]
        [HttpPost]
        public IHttpActionResult AddWorkout(Workout workout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Workouts.Add(workout);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = workout.WorkoutId }, workout);
        }

        /// <summary>
        /// deletes a workout from the system by inputing its workoutId
        /// </summary>
        /// <param name="id">Id is the Primary key of workout</param>
        /// <returns> Header: 200(Okay) or Header: 404(not found) </returns>
        /// <example>
        /// POST (Delete): api/WorkoutData/DeleteWorkout/5
        /// Form Data: [empty]
        /// </example>
    
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
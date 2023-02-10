using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using FitnessApp.Models;

namespace FitnessApp.Controllers
{
    public class WorkoutController : Controller
    {
        // GET: Workout/List
        public ActionResult List()
        {
            //objective: communicate with our workout data api to retrieve a list of workout
            //curl https://localhost:44376/api/workoutdata/workoutlist

            //client is anything that is accessing info in the server, and can also exist in the server to send a request to our data access API and anticipate a response
            HttpClient client = new HttpClient() { };
            //establish URL communication endpoint
            string url = "https://localhost:44376/api/workoutdata/workoutlist";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The request is ");
            Debug.WriteLine(response.StatusCode);

            //parse message into IEnumerable
            IEnumerable<WorkoutDto> Workouts = response.Content.ReadAsAsync<IEnumerable<WorkoutDto>>().Result;
            Debug.WriteLine("Number of workouts are : ");
            Debug.WriteLine(Workouts.Count());


            return View(Workouts);
        }

        // GET: Workout/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our workout data api to retrieve a workout
            //curl https://localhost:44376/api/workoutdata/findworkout/{id}

            HttpClient client = new HttpClient() { };
            //establish URL communication endpoint
            string url = "https://localhost:44376/api/workoutdata/findworkout/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The request is ");
            Debug.WriteLine(response.StatusCode);

            //parse message into IEnumerable
            WorkoutDto selectedWorkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
            Debug.WriteLine("Workout Found is : ");
            Debug.WriteLine(selectedWorkout.WorkoutName);

            return View(selectedWorkout);
        }

        // GET: Workout/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Workout/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Workout/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Workout/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Workout/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Workout/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

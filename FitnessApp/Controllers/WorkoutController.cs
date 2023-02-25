﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using FitnessApp.Models;
using System.Web.Script.Serialization;


namespace FitnessApp.Controllers
{
    public class WorkoutController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static WorkoutController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44376/api/workoutdata/");
        }
        
        // GET: Workout/List
        public ActionResult List()
        {
            //objective: communicate with our workout data api to retrieve a list of workout
            //curl https://localhost:44376/api/workoutdata/workoutlist

            //client is anything that is accessing info in the server, and can also exist in the server to send a request to our data access API and anticipate a response
            

            //establish URL communication endpoint
            string url = "workoutlist";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The request is ");
            //Debug.WriteLine(response.StatusCode);

            //parse message into IEnumerable
            IEnumerable<WorkoutDto> Workouts = response.Content.ReadAsAsync<IEnumerable<WorkoutDto>>().Result;
            

            return View(Workouts);
        }

        // GET: Workout/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our workout data api to retrieve a workout
            //curl https://localhost:44376/api/workoutdata/findworkout/{id}

            //establish URL communication endpoint
            string url = "findworkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The request is ");
            Debug.WriteLine(response.StatusCode);

            //parse message into IEnumerable
            WorkoutDto selectedWorkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
            Debug.WriteLine("Workout Found is : ");
            Debug.WriteLine(selectedWorkout.WorkoutName);

            return View(selectedWorkout);
        }

        // GET: Workout/New
        public ActionResult New()
        {
            
            return View();
        }

        // POST: Workout/Create
        [HttpPost]
        public ActionResult Create(Workout workout)
        {
            //Debug.WriteLine("Inputted workout is: ");
            //Debug.WriteLine(workout.WorkoutName);

            //add new workout into the system through API
            //curl -H "Content-Type: application/json" -d @workout.json https://localhost:44376/api/workoutdata/addworkout
            //string url = "addworkout";

            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //string jsonpayload = jss.Serialize(workout); //this will convert obj to a json string

            ////Debug.WriteLine("JSON Payload is: ");
            ////Debug.WriteLine(jsonpayload);

            ////HttpContent content = new StringContent(jsonpayload);
            ////content.Headers.ContentType.MediaType = "application/json";

            ////client.PostAsync(url, content);

                return RedirectToAction("List");
           

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using FitnessApp.Models;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FitnessApp.Controllers
{
    public class WorkoutController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static WorkoutController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44376/api/");
        }

        // GET: Workout/List
        public async Task<ActionResult> List()
        {
            //objective: communicate with our workout data api to retrieve a list of workout
            //curl https://localhost:44376/api/workoutdata/workoutlist

            //client is anything that is accessing info in the server, and can also exist in the server to send a request to our data access API and anticipate a response


            //establish URL communication endpoint
            string url = "workoutdata/workoutlist";
            HttpResponseMessage response = await client.GetAsync(url);

            //Debug.WriteLine("The request is ");
            //Debug.WriteLine(response.StatusCode);

            //parse message into IEnumerable
            //if response is  successfull
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                IEnumerable<WorkoutDto> Workouts = JsonConvert.DeserializeObject<List<WorkoutDto>>(json);
                return View(Workouts);
            }
            return View(new List<WorkoutDto>());

        }

        // GET: Workout/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our workout data api to retrieve a workout
            //curl https://localhost:44376/api/workoutdata/findworkout/{id}

            //establish URL communication endpoint
            string url = "workoutdata/findworkout/" + id;
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
            string url = "workoutdata/addworkout";

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(workout); //this will convert obj to a json string

            ////Debug.WriteLine("JSON Payload is: ");
            ////Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            client.PostAsync(url, content);

            return RedirectToAction("List");


        }

        // GET: Workout/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "workoutdata/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            WorkoutDto selectedWorkout = response.Content.ReadAsAsync<WorkoutDto>().Result;

            return View(selectedWorkout);
        }

        public ActionResult Error()
        {
            return View();
        }

        // POST: Workout/Update/5
        [HttpPost]
        public ActionResult Edit(int id, Workout workout)
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


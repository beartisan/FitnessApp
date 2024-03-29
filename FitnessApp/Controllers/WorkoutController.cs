﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using FitnessApp.Models;
using FitnessApp.Models.ViewModels;
using System.Web.Script.Serialization;
//using FitnessApp.Migrations;


namespace FitnessApp.Controllers
{
    public class WorkoutController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static WorkoutController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
            //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44376/api/");
        }

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Workout/List
        public ActionResult List()
        {
            //objective: communicate with our workout data api to retrieve a list of workout
            //curl https://localhost:44376/api/workoutdata/workoutlist

            //client is anything that is accessing info in the server, and can also exist in the server to send a request to our data access API and anticipate a response


            //establish URL communication endpoint
            string url = "workoutdata/workoutlist";
            HttpResponseMessage response =  client.GetAsync(url).Result;

        //Debug.WriteLine("The request is ");
        //Debug.WriteLine(response.StatusCode);

        //parse message into IEnumerable
        IEnumerable<WorkoutDto> Workouts = response.Content.ReadAsAsync<IEnumerable<WorkoutDto>>().Result;

        return View(Workouts);

    }

        // GET: Workout/Details/5

        public ActionResult Details(int id)
        {
            WorkoutDetails ViewModel = new WorkoutDetails();

            //objective: communicate with our workout data api to retrieve a workout
            //curl https://localhost:44376/api/workoutdata/findworkout/{id}

            //establish URL communication endpoint
            string url = "workoutdata/findworkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

           // Debug.WriteLine("The request is ");
           // Debug.WriteLine(response.StatusCode);

            //parse message into IEnumerable
            WorkoutDto SelectedWorkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
           // Debug.WriteLine("Workout Found is : ");
            ViewModel.SelectedWorkout = SelectedWorkout;

            //Shows Athletes that are sharing specific workout
            url = "Athletedata/ListAthletesWithThisWorkout/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AthleteDto> AssociatedAthletes = response.Content.ReadAsAsync<IEnumerable<AthleteDto>>().Result;

            ViewModel.AssociatedAthletes = AssociatedAthletes;

            //Shows Athletes that are not connected or doing this specific workout
            url = "Athletedata/ListAthletesWithoutThisWorkout/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AthleteDto> UnassociatedAthletes = response.Content.ReadAsAsync<IEnumerable<AthleteDto>>().Result;

            ViewModel.UnassociatedAthletes = UnassociatedAthletes;

            return View(ViewModel);
        }

        //POST: Workout/Associate/{id}?AthleteId={AthleteId}
        [Authorize]
        [HttpPost]
        public ActionResult Associate(int id, int AthleteId)
        {
            GetApplicationCookie();
            Debug.WriteLine("Associated Workout is: " + id + " and their Athlete " + AthleteId);

            //Call API that associates the workout with Athletes
            string url = "workoutdata/associateworkoutwithAthlete/" + id + "/" + AthleteId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);

        }

        //POST: Workout/Unassociate/{id}?AthleteId={AthleteId}
        [Authorize]
        [HttpPost]
        public ActionResult Unassociate(int id, int AthleteId)
        {
            GetApplicationCookie();
            Debug.WriteLine("Unassociated Workout is: " + id + " and their unassociated Athlete " + AthleteId);

            //Call API that the workout is not associated with Athletes
            string url = "workoutdata/unassociateworkoutwithAthlete/" + id + "/" + AthleteId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        // GET: Workout/New
        [Authorize]
        public ActionResult New()
        {
            //information about ALL categories in the system
            // GET: api/categorydata/listcategory

            string url = "categorydata/listcategory";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CategoriesDto> CategoryOptions = response.Content.ReadAsAsync<IEnumerable<CategoriesDto>>().Result;

            return View(CategoryOptions);
        }

        // POST: Workout/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Workout workout)
        {
            GetApplicationCookie();
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

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Workout/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            UpdateWorkout ViewModel = new UpdateWorkout();

            //existing workout information
            string url = "workoutdata/findworkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WorkoutDto SelectedWorkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
            ViewModel.SelectedWorkout = SelectedWorkout;

            //include ALL category to choose from when updating workout
            url = "categorydata/categorylist";
            response = client.GetAsync(url).Result;
            IEnumerable<CategoriesDto> CategoryOptions = response.Content.ReadAsAsync<IEnumerable<CategoriesDto>>().Result;

            ViewModel.CategoryOptions = CategoryOptions;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // POST: Workout/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, Workout workout)
        {
            GetApplicationCookie();
            //Debug.WriteLine("Inputted workout is: ");
            //Debug.WriteLine(workout.WorkoutName);

            //update  workout into the system through API
            //curl -H "Content-Type: application/json" -d @workout.json https://localhost:44376/api/workoutdata/addworkout
            string url = "workoutdata/updateworkout/" + id;

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(workout); 

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Workout/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "workoutdata/findworkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WorkoutDto SelectedWorkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
            
            return View(SelectedWorkout);
        }

        // POST: Workout/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();

            //delete  workout into the system through API
            //curl -H "Content-Type: application/json" -d @workout.json https://localhost:44376/api/workoutdata/deleteworkout/1
            string url = "workoutdata/deleteworkout/" + id;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }
    }
}


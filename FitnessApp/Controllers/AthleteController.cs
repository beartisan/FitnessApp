using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using FitnessApp.Models;
using FitnessApp.Models.ViewModels;
using System.Web.Script.Serialization;

namespace FitnessApp.Controllers
{
    public class AthleteController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AthleteController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44376/api/");
        }
   
            // GET: Athlete/List
            public ActionResult Index()
        {
            //objective: communicate with our athlete data api to retrieve a list of athletes
            //curl https://localhost:44376/api/athletedata/listathletes

            string url = "athletedata/listathletes";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<AthleteDto> Athletes = response.Content.ReadAsAsync<IEnumerable<AthleteDto>>().Result;

            return View(Athletes);
        }

        // GET: Athlete/Details/5
        public ActionResult Details(int id)
        {
            AthleteDetails ViewModel = new AthleteDetails();

            //objective: communicate with our athlete data api to retrieve one athlete
            //curl https://localhost:44376/api/athletedata/findAthlete/{id}

            string url = "athletedata/findathlete/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AthleteDto SelectedAthlete = response.Content.ReadAsAsync<AthleteDto>().Result;
            ViewModel.SelectedAthlete = SelectedAthlete;

            //showcase info about workout related to this athlete
            //send a request to gather information about workout related to particular athlete Id
            url = "workoutdata/WorkoutListForAthlete/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<WorkoutDto> CurrentWorkout = response.Content.ReadAsAsync<IEnumerable<WorkoutDto>>().Result;

            ViewModel.CurrentWorkout = CurrentWorkout;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Athlete/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Athlete/Create
        [HttpPost]
        public ActionResult Create(Athlete Athlete)
        {
            //objective: add a new athlete into the system using API
            //curl -H "Content-Type: application/json" -d @Athlete.json https://localhost:44376/api/athletedata/addathlete
            string url = "athletedata/addathlete";

            string jsonpayload = jss.Serialize(Athlete);
            Debug.WriteLine(jsonpayload);

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

        // GET: Athlete/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "athletedata/findathlete/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AthleteDto SelectedAthlete = response.Content.ReadAsAsync<AthleteDto>().Result;

            return View(SelectedAthlete);
        }

        // POST: Athlete/Update/5
        [HttpPost]
        public ActionResult Update(int id, Athlete Athlete)
        {
            //objective: communicate with our athlete data api to retrieve one athlete
            //curl -H "Content-Type: application/json" -d @Athlete.json https://localhost:44376/api/athletedata/updateathlete/{id}

            AthleteDetails ViewModel = new AthleteDetails();

            string url = "athletedata/updateathlete/" + id;
            string jsonpayload = jss.Serialize(Athlete);

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

        // GET: Athlete/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "athletedata/findathlete/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AthleteDto SelectedAthlete = response.Content.ReadAsAsync<AthleteDto>().Result;

            return View(SelectedAthlete);
        }

        // POST: Athlete/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //objective: delete the Athlete through API using their AthleteID
            ///curl -H "Content-Type: application/json" -d @Athlete.json https://localhost:44376/api/athletedata/deleteathlete/{id}

            string url = "athletedata/deleteathlete/" + id;

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

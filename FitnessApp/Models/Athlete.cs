﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class Athlete
    {
        [Key]
        public int AthleteId { get; set; }

        public string AthleteFirstName { get; set; }

        public string AthleteLastName { get; set; }

        //athlete can have a lot of workouts
        public ICollection<Workout> Workouts { get; set; }



    }
}
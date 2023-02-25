using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Models
{
    public class Workout
    {
        [Key]
        public int WorkoutId { get; set; }

        public string WorkoutName { get; set; }

        //date only
        public DateTime WorkoutDate { get; set; }

        //time in minutes
        public int WorkoutDuration { get; set; }

        //Workout foreign key to category entity
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        //public string CategoryName { get; set; }

        //trainer id



        //a Workout have a lot of athlete
        //athlete can have a lot of Workouts
        public ICollection<Athlete> Athletes { get; set; }

        //location id

    }

    public class WorkoutDto
    {
        public int WorkoutId { get; set; }

        public string WorkoutName { get; set; }

        //date only
        public DateTime WorkoutDate { get; set; }

        //time in minutes
        public int WorkoutDuration { get; set; }

        public int CategoryId { get; set; }
        
        public string CategoryName { get; set; }

    }

}
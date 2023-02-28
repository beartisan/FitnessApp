using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessApp.Models.ViewModels
{
    public class UpdateWorkout
    {
        //This Viewmodel is a class --> Stores info we need to present to /workout/update/{id}

        //existing workout information
        public WorkoutDto selectedWorkout { get; set; }

        //include ALL category to choose from when updating workout

        public IEnumerable<CategoriesDto> CategoryOptions { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessApp.Models.ViewModels
{
    public class CategoryDetails
    {
        internal IEnumerable<WorkoutDto> relatedWorkout;

        public CategoriesDto SelectedCategory { get; set; }
        public IEnumerable<WorkoutDto> RelatedWorkout { get; set; }
    }
}
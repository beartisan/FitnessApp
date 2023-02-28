using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessApp.Models.ViewModels
{
    public class CategoryDetails
    {
        public CategoriesDto SelectedCategory { get; set; }
        public IEnumerable<WorkoutDto> RelatedWorkouts { get; set; }
    }
}
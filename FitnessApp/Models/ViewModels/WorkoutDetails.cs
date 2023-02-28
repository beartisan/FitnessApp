using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessApp.Models.ViewModels
{
    public class WorkoutDetails
    {
        public WorkoutDto SelectedWorkout { get; set; }
        public IEnumerable<AthleteDto> AssociatedAthletes { get; set; }
        public IEnumerable<AthleteDto> UnassociatedAthletes { get; set; }
    }
}
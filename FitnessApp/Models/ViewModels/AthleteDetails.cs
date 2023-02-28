using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessApp.Models.ViewModels
{
    public class AthleteDetails
    {
        public AthleteDto SelectedAthlete { get; set; }
        public IEnumerable<WorkoutDto> CurrentWorkout { get; set; }
    }
}
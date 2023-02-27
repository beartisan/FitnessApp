namespace FitnessApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class latestmigration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AthleteWorkouts", newName: "WorkoutAthletes");
            DropPrimaryKey("dbo.WorkoutAthletes");
            AddPrimaryKey("dbo.WorkoutAthletes", new[] { "Workout_WorkoutId", "Athlete_AthleteId" });
            DropColumn("dbo.AspNetUsers", "CategoryName");
            DropColumn("dbo.Workouts", "CategoryName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Workouts", "CategoryName", c => c.String());
            AddColumn("dbo.AspNetUsers", "CategoryName", c => c.String());
            DropPrimaryKey("dbo.WorkoutAthletes");
            AddPrimaryKey("dbo.WorkoutAthletes", new[] { "Athlete_AthleteId", "Workout_WorkoutId" });
            RenameTable(name: "dbo.WorkoutAthletes", newName: "AthleteWorkouts");
        }
    }
}

namespace FitnessApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class athletesworkouts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Athletes",
                c => new
                    {
                        AthleteId = c.Int(nullable: false, identity: true),
                        AthleteFirstName = c.String(),
                        AthleteLastName = c.String(),
                    })
                .PrimaryKey(t => t.AthleteId);
            
            CreateTable(
                "dbo.AthleteWorkouts",
                c => new
                    {
                        Athlete_AthleteId = c.Int(nullable: false),
                        Workout_WorkoutId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Athlete_AthleteId, t.Workout_WorkoutId })
                .ForeignKey("dbo.Athletes", t => t.Athlete_AthleteId, cascadeDelete: true)
                .ForeignKey("dbo.Workouts", t => t.Workout_WorkoutId, cascadeDelete: true)
                .Index(t => t.Athlete_AthleteId)
                .Index(t => t.Workout_WorkoutId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AthleteWorkouts", "Workout_WorkoutId", "dbo.Workouts");
            DropForeignKey("dbo.AthleteWorkouts", "Athlete_AthleteId", "dbo.Athletes");
            DropIndex("dbo.AthleteWorkouts", new[] { "Workout_WorkoutId" });
            DropIndex("dbo.AthleteWorkouts", new[] { "Athlete_AthleteId" });
            DropTable("dbo.AthleteWorkouts");
            DropTable("dbo.Athletes");
        }
    }
}

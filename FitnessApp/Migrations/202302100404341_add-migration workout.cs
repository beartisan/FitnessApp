namespace FitnessApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmigrationworkout : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Workouts", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Workouts", "CategoryId");
            AddForeignKey("dbo.Workouts", "CategoryId", "dbo.Categories", "CategoryId", cascadeDelete: true);
            DropColumn("dbo.Workouts", "WorkoutCategory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Workouts", "WorkoutCategory", c => c.String());
            DropForeignKey("dbo.Workouts", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Workouts", new[] { "CategoryId" });
            DropColumn("dbo.Workouts", "CategoryId");
        }
    }
}

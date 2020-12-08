namespace BigJobbs.Migrations
{
    using BigJobbs.Models;
    using BigJobbs.Services;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Web.Configuration;

    public partial class SeedJob : DbMigration
    {
        readonly Context DbContext;
        public SeedJob(Context ctx)
        {
            DbContext = ctx;
        }
        public override void Up()
        {
            // Seed a default Job
            var job = new Job
            {
                Name = "UI/UX Designer",
                Description = "It is a long established fact that a reader will beff distracted by vbthe creadable content of a page when looking at its layout. The pointf of using Lorem Ipsum is that it has ahf mcore or-lgess normal distribution of letters, as opposed to using, Content here content here making it look like readable.",
                Salary = 150000,
                HiringCompany = "Ziggo",
                Location = "Abuja",
                JobCategoryId = 5,
                JobTypeId = 1,
                DatePosted = DateTime.Now
            };
            DbContext._dbContext.Jobs.Add(job);
            DbContext._dbContext.SaveChanges();

        }
        
        public override void Down()
        {
        }
    }
}

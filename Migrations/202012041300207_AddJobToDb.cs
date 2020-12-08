namespace BigJobbs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Salary = c.Int(nullable: false),
                        DatePosted = c.DateTime(nullable: false),
                        HiringCompany = c.String(),
                        Location = c.String(),
                        JobCategoryId = c.Int(nullable: false),
                        JobTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobCategories", t => t.JobCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.JobTypes", t => t.JobTypeId, cascadeDelete: true)
                .Index(t => t.JobCategoryId)
                .Index(t => t.JobTypeId);
            
            CreateTable(
                "dbo.JobCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "JobTypeId", "dbo.JobTypes");
            DropForeignKey("dbo.Jobs", "JobCategoryId", "dbo.JobCategories");
            DropIndex("dbo.Jobs", new[] { "JobTypeId" });
            DropIndex("dbo.Jobs", new[] { "JobCategoryId" });
            DropTable("dbo.JobTypes");
            DropTable("dbo.JobCategories");
            DropTable("dbo.Jobs");
        }
    }
}

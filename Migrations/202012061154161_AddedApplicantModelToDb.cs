namespace BigJobbs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedApplicantModelToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applicants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Sex = c.Byte(nullable: false),
                        UserId = c.Int(nullable: false),
                        JobId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.JobId)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Applicants", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Applicants", "JobId", "dbo.Jobs");
            DropIndex("dbo.Applicants", new[] { "User_Id" });
            DropIndex("dbo.Applicants", new[] { "JobId" });
            DropTable("dbo.Applicants");
        }
    }
}

namespace BigJobbs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobApplicationStatusToApplicant : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Applicants", new[] { "User_Id" });
            DropColumn("dbo.Applicants", "UserId");
            RenameColumn(table: "dbo.Applicants", name: "User_Id", newName: "UserId");
            AddColumn("dbo.Applicants", "JobApplicationStatus", c => c.String());
            AlterColumn("dbo.Applicants", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Applicants", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Applicants", new[] { "UserId" });
            AlterColumn("dbo.Applicants", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Applicants", "JobApplicationStatus");
            RenameColumn(table: "dbo.Applicants", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Applicants", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Applicants", "User_Id");
        }
    }
}

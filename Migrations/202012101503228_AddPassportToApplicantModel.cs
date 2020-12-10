namespace BigJobbs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPassportToApplicantModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applicants", "PassportPath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applicants", "PassportPath");
        }
    }
}

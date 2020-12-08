namespace BigJobbs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedSexFromApplicant : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Applicants", "Sex");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Applicants", "Sex", c => c.String(nullable: false));
        }
    }
}

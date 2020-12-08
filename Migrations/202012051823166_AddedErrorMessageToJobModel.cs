namespace BigJobbs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedErrorMessageToJobModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Jobs", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Jobs", "HiringCompany", c => c.String(nullable: false));
            AlterColumn("dbo.Jobs", "Location", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "Location", c => c.String());
            AlterColumn("dbo.Jobs", "HiringCompany", c => c.String());
            AlterColumn("dbo.Jobs", "Description", c => c.String());
            AlterColumn("dbo.Jobs", "Name", c => c.String());
        }
    }
}

namespace BigJobbs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageToJobModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "ImagePath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "ImagePath");
        }
    }
}

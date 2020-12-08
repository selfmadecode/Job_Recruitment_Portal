namespace BigJobbs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedSexToStringInApplicant : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Applicants", "Sex", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Applicants", "Sex", c => c.Byte(nullable: false));
        }
    }
}

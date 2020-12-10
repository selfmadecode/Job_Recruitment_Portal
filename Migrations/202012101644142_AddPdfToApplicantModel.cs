namespace BigJobbs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPdfToApplicantModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applicants", "PdfPath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applicants", "PdfPath");
        }
    }
}

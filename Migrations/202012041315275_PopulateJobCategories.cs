namespace BigJobbs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateJobCategories : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO JobCategories (Name) VALUES ('Design & Creative')");
            Sql("INSERT INTO JobCategories (Name) VALUES ('Design & Development')");
            Sql("INSERT INTO JobCategories (Name) VALUES ('Sales & Marketing')");
            Sql("INSERT INTO JobCategories (Name) VALUES ('Construction')");
            Sql("INSERT INTO JobCategories (Name) VALUES ('IT')");
            Sql("INSERT INTO JobCategories (Name) VALUES ('Real Estate')");
            Sql("INSERT INTO JobCategories (Name) VALUES ('Content Writer')");
            Sql("INSERT INTO JobCategories (Name) VALUES ('Transport')");
            Sql("INSERT INTO JobCategories (Name) VALUES ('Others')");
        }
        
        public override void Down()
        {
        }
    }
}

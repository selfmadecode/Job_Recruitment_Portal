namespace BigJobbs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateJobType : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO JobTypes (Name) VALUES ('Full Time')");
            Sql("INSERT INTO JobTypes (Name) VALUES ('Part Time')");
            Sql("INSERT INTO JobTypes (Name) VALUES ('Remote')");
            Sql("INSERT INTO JobTypes (Name) VALUES ('Free Lance')");
        }
        
        public override void Down()
        {
        }
    }
}

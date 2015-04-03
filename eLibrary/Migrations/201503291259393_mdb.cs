namespace eLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mdb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Authors", "Image");
        }
    }
}

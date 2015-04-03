namespace eLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mdb : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Authors", "Patronymic", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Authors", "Patronymic", c => c.String(nullable: false, maxLength: 50));
        }
    }
}

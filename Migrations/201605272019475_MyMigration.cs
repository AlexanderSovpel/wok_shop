namespace WokShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MyMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Foods", "Count", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Foods", "Count");
        }
    }
}

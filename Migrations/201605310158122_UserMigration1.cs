namespace WokShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserMigration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                {
                    UserId = c.Int(nullable: false, identity: true),
                    Email = c.String(),
                    Password = c.String(),
                    Role = c.String(),
                })
                .PrimaryKey(t => t.UserId);
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");

        }
    }
}

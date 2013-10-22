namespace Svitla.MovieService.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailQueue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Email",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Subject = c.String(),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Recipient",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Email = c.String(),
                        Name = c.String(),
                        Role = c.Int(nullable: false),
                        Email_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Email", t => t.Email_Id, cascadeDelete: true)
                .Index(t => t.Email_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Recipient", new[] { "Email_Id" });
            DropForeignKey("dbo.Recipient", "Email_Id", "dbo.Email");
            DropTable("dbo.Recipient");
            DropTable("dbo.Email");
        }
    }
}

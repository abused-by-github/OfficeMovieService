namespace Svitla.MovieService.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPermissions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserPermission",
                c => new
                    {
                        User_Id = c.Long(nullable: false),
                        Permission_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Permission_Id })
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Permission", t => t.Permission_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Permission_Id);
            
        }

        public override void Down()
        {
            DropIndex("dbo.UserPermission", new[] { "Permission_Id" });
            DropIndex("dbo.UserPermission", new[] { "User_Id" });
            DropForeignKey("dbo.UserPermission", "Permission_Id", "dbo.Permission");
            DropForeignKey("dbo.UserPermission", "User_Id", "dbo.User");
            DropTable("dbo.UserPermission");
            DropTable("dbo.Permission");
        }
    }
}

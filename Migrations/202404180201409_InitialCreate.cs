namespace Property_Rental_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        PropertyID = c.Int(nullable: false, identity: true),
                        NumberOfRooms = c.Int(nullable: false),
                        Rent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        ManagerID = c.Int(nullable: false),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.PropertyID)
                .ForeignKey("dbo.Users", t => t.User_UserID)
                .Index(t => t.User_UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        UserType = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentID = c.Int(nullable: false, identity: true),
                        DateAndTime = c.DateTime(nullable: false),
                        Description = c.String(),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppointmentID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        SenderID = c.Int(nullable: false),
                        RecipientID = c.Int(nullable: false),
                        User_UserID = c.Int(),
                        User1_UserID = c.Int(),
                        User_UserID1 = c.Int(),
                        User_UserID2 = c.Int(),
                    })
                .PrimaryKey(t => t.MessageID)
                .ForeignKey("dbo.Users", t => t.User_UserID)
                .ForeignKey("dbo.Users", t => t.User1_UserID)
                .ForeignKey("dbo.Users", t => t.User_UserID1)
                .ForeignKey("dbo.Users", t => t.User_UserID2)
                .Index(t => t.User_UserID)
                .Index(t => t.User1_UserID)
                .Index(t => t.User_UserID1)
                .Index(t => t.User_UserID2);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Properties", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.Messages", "User_UserID2", "dbo.Users");
            DropForeignKey("dbo.Messages", "User_UserID1", "dbo.Users");
            DropForeignKey("dbo.Messages", "User1_UserID", "dbo.Users");
            DropForeignKey("dbo.Messages", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.Appointments", "UserID", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "User_UserID2" });
            DropIndex("dbo.Messages", new[] { "User_UserID1" });
            DropIndex("dbo.Messages", new[] { "User1_UserID" });
            DropIndex("dbo.Messages", new[] { "User_UserID" });
            DropIndex("dbo.Appointments", new[] { "UserID" });
            DropIndex("dbo.Properties", new[] { "User_UserID" });
            DropTable("dbo.Messages");
            DropTable("dbo.Appointments");
            DropTable("dbo.Users");
            DropTable("dbo.Properties");
        }
    }
}

namespace Property_Rental_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePropertyModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Apartments",
                c => new
                    {
                        ApartmentID = c.Int(nullable: false, identity: true),
                        NumberOfRooms = c.Int(),
                        Rent = c.Decimal(precision: 18, scale: 2),
                        Status = c.String(),
                        PropertyID = c.Int(),
                    })
                .PrimaryKey(t => t.ApartmentID)
                .ForeignKey("dbo.Properties", t => t.PropertyID)
                .Index(t => t.PropertyID);
            
            AddColumn("dbo.Properties", "Address", c => c.String());
            AddColumn("dbo.Properties", "Name", c => c.String());
            DropColumn("dbo.Properties", "NumberOfRooms");
            DropColumn("dbo.Properties", "Rent");
            DropColumn("dbo.Properties", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Properties", "Status", c => c.String());
            AddColumn("dbo.Properties", "Rent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Properties", "NumberOfRooms", c => c.Int(nullable: false));
            DropForeignKey("dbo.Apartments", "PropertyID", "dbo.Properties");
            DropIndex("dbo.Apartments", new[] { "PropertyID" });
            DropColumn("dbo.Properties", "Name");
            DropColumn("dbo.Properties", "Address");
            DropTable("dbo.Apartments");
        }
    }
}

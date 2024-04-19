using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Property_Rental_Management.Data
{
    public class Property_Rental_ManagementContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public Property_Rental_ManagementContext() : base("name=Property_Rental_ManagementContext")
        {
        }

        public System.Data.Entity.DbSet<Property_Rental_Management.Models.Property> Properties { get; set; }

        public System.Data.Entity.DbSet<Property_Rental_Management.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<Property_Rental_Management.Models.Appointment> Appointments { get; set; }

        public System.Data.Entity.DbSet<Property_Rental_Management.Models.Message> Messages { get; set; }

        public System.Data.Entity.DbSet<Property_Rental_Management.Models.Apartment> Apartments { get; set; }
    }
}

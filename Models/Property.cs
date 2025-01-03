//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PropertyRentalManagementWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Property
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Property()
        {
            this.Apartments = new HashSet<Apartment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PropertyID { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Only letters, numbers, and spaces are allowed.")]
        public string Address { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string Name { get; set; }

        public int ManagerID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Apartment> Apartments { get; set; }
        public virtual User User { get; set; }
    }
}

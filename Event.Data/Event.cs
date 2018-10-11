//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Event.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Event
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Event()
        {
            this.EventMembers = new HashSet<EventMember>();
        }
    
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Vanue { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.TimeSpan StartTime { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.TimeSpan EndTime { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public bool IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventMember> EventMembers { get; set; }
        public virtual Company Company { get; set; }
        public virtual User User { get; set; }
    }
}

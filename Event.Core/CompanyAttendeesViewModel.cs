namespace Event.Core
{
    using System;

    public class CompanyAttendeesViewModel
    {
        public long CompanyId { get; set; }

        public DateTime CreatedDate { get; set; }

        public long Id { get; set; }

        public bool IsActive { get; set; }

        public long UserId { get; set; }
    }
}
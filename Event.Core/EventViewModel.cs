namespace Event.Core
{
    using System;

    public class EventViewModel
    {
        public long CompanyId { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Description { get; set; }

        public string EndDate { get; set; }

        public string EndTime { get; set; }

        public long Id { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public string StartDate { get; set; }

        public string StartTime { get; set; }

        public string Vanue { get; set; }
    }
}
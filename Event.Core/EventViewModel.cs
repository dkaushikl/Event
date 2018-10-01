namespace Event.Core
{
    using System;

    public class EventViewModel
    {
        public long CompanyId { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Description { get; set; }

        public DateTime EndDate { get; set; }

        public TimeSpan EndTime { get; set; }

        public long Id { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public string Vanue { get; set; }
    }
}
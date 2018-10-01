namespace Event.Core
{
    using System;

    public class EventAttendeesViewModel
    {
        public DateTime CreatedDate { get; set; }

        public long EventId { get; set; }

        public long Id { get; set; }

        public bool IsActive { get; set; }

        public long UserId { get; set; }
    }
}
namespace Event.Core
{
    using System;

    public class EventMemberViewModel
    {
        public DateTime CreatedDate { get; set; }

        public long EventId { get; set; }

        public long Id { get; set; }

        public bool IsActive { get; set; }

        public long UserId { get; set; }

        public string EventName { get; set; }
    }
}
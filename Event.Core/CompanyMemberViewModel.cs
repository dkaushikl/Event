namespace Event.Core
{
    using System;

    public class CompanyMemberViewModel
    {
        public long CompanyId { get; set; }

        public DateTime CreatedDate { get; set; }

        public long Id { get; set; }

        public bool IsActive { get; set; }

        public long UserId { get; set; }

        public string Email { get; set; }
    }
}
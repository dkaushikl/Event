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

    public partial class CompanyMemberDisplayViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string UserEmail { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
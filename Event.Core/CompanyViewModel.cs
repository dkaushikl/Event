namespace Event.Core
{
    using System;

    public class CompanyViewModel
    {
        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Email { get; set; }

        public long Id { get; set; }

        public bool IsActive { get; set; }

        public string MobileNo { get; set; }

        public string Name { get; set; }

        public string State { get; set; }
    }

    public class CompanyDisplayViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string CompanyEmail { get; set; }

        public string MobileNo { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UserEmail { get; set; }
    }
}
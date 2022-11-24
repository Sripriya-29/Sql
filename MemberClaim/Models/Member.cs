using System;
using System.Collections.Generic;

namespace MemberClaim.Models
{
    public partial class Member
    {
        public Member()
        {
            Claims = new HashSet<Claim>();
            Dependents = new HashSet<Dependent>();
        }

        public int Id { get; set; }
        public string? MemberId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Email { get; set; }
        public string? Pan { get; set; }
        public int? ContactNo { get; set; }
        public DateTime? Dob { get; set; }
        public string? Password { get; set; }

        public virtual ICollection<Claim> Claims { get; set; }
        public virtual ICollection<Dependent> Dependents { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace MemberClaim.Models
{
    public partial class Claim
    {
        public int ClaimId { get; set; }
        public int? MemberId { get; set; }
        public string? MemberName { get; set; }
        public DateTime? Dob { get; set; }
        public DateTime? DateOfAdmission { get; set; }
        public DateTime? DateOfDischarge { get; set; }
        public string? ProviderName { get; set; }
        public double? BillAmount { get; set; }

        public virtual Member? Member { get; set; }
    }
}

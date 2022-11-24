using System;
using System.Collections.Generic;

namespace ClaimServices.Models
{
    public partial class Dependent
    {
        public int DependentId { get; set; }
        public string? Name { get; set; }
        public DateTime? Dob { get; set; }
        public int? MemberId { get; set; }

        public virtual Member? Member { get; set; }
    }
}

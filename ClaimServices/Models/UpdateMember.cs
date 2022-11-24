using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ClaimServices.Models
{
    public partial class UpdateMember
    {
        //public Member()
        //{
        //    Claims = new HashSet<Claim>();
        //    Dependents = new HashSet<Dependent>();
        //}

      
        public string? Address { get; set; }
        public string? State { get; set; }
     
        public string? Email { get; set; }
        public string? Pan { get; set; }
        public int? ContactNo { get; set; }
       
    }
}

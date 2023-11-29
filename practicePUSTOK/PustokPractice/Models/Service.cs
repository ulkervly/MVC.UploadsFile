using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Collections.Specialized.BitVector32;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System;
using System.ComponentModel.DataAnnotations;

namespace PustokPractice.Models
{
    public class Service
    {
        public int id { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Title { get; set; }
        [Required]
        [StringLength(maximumLength: 500)]
        public string Description { get; set; }
        public string Icon { get; set; }
        
    }
}

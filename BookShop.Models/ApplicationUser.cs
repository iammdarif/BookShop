using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        public string? StreetAddress { get; set; }
        [MaxLength(30)]
        public string? City { get; set; }
        [MaxLength(15)]
        public string? State { get; set; }
        [MaxLength(10)]
        public string? PostCode { get; set; }       
    }
}

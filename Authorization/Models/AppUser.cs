using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authorization.Models
{
    public class AppUser: IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(15)")]
        public string FullName { get; set; }
    }
}

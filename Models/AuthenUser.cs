using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace razor_page_ef
{
    public class AuthenUser : IdentityUser
    {
        [DataType(DataType.Date)]
        public DateTime ?DateBirth { get; set; }
    }
}
using Microsoft.AspNetCore.Authorization;

namespace App.Authorize.Requiremnts
{
    public class AuthorizationRequimentEdit : IAuthorizationRequirement {
        public DateTime CurrDate { get; set; }
        public AuthorizationRequimentEdit(){
            CurrDate = DateTime.Now;
        }
    }
}
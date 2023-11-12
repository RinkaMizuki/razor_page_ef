using Microsoft.AspNetCore.Authorization;

namespace App.Authorize.Requiremnts
{
    public class AuthorizationRequimentGenZ : IAuthorizationRequirement
    {
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public AuthorizationRequimentGenZ(int _FromYear = 1995, int _ToYear = 2007)
        {
            FromYear = _FromYear;
            ToYear = _ToYear;
        }
    }
}
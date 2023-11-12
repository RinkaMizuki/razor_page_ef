using System.Security.Claims;
using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using razor_page_ef;

namespace App.Authorize.Requiremnts
{
    public class AppAuthorizationHandler : IAuthorizationHandler
    {
        private readonly UserManager<AuthenUser> _userManager;
        private readonly ILogger<AppAuthorizationHandler> _logger;
        public AppAuthorizationHandler(UserManager<AuthenUser> userManager, ILogger<AppAuthorizationHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            var userInfo = await _userManager.GetUserAsync(context.User);
            foreach (var requirement in context.PendingRequirements)
            {
                if (requirement is AuthorizationRequimentGenZ)
                {
                    var requireGenZ = requirement as AuthorizationRequimentGenZ;
                    if (checkBirthUser(requireGenZ, userInfo))
                    {
                        context.Succeed(requirement);
                    }
                }
                if (requirement is AuthorizationRequimentEdit)
                {
                    var requireEdit = requirement as AuthorizationRequimentEdit;
                    if (checkEditBlog(context.User, context.Resource, requireEdit))
                    {
                        context.Succeed(requirement);
                    }
                }
            }
        }

        private bool checkEditBlog(ClaimsPrincipal user,object resource, AuthorizationRequimentEdit requireEdit)
        {
            //IsInRole được lấy từ ClaimsPrincipal User để check Role nào đó của User có tồn tại hay k
            if (user.IsInRole("Administrators")) return true;
            var article = resource as Article;
            return article.PublishedDate.Year <= requireEdit.CurrDate.Year;
        }

        private bool checkBirthUser(AuthorizationRequimentGenZ requireGenZ, AuthenUser userInfo)
        {
            if (userInfo.DateBirth == null)
            {
                _logger.LogInformation($"{userInfo.UserName} không có ngày sinh");
                return false;
            }
            int year = userInfo.DateBirth.Value.Year;
            bool isSuccess = year >= requireGenZ.FromYear & year <= requireGenZ.ToYear;
            if (isSuccess)
            {
                _logger.LogInformation($"{userInfo.UserName} đủ tuổi truy cập");
            }
            else
            {
                _logger.LogInformation($"{userInfo.UserName} không đủ tuổi truy cập");
            }
            return isSuccess;
        }
    }
}
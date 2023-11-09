using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace App.Admin.Roles
{
    public class CreateModel : RolePageModel
    {
        public class InputModel
        {
            [DisplayName("Tên role")]
            [Required, StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} kí tự")]
            public string Name { get; set; }
        }
        public CreateModel(RoleManager<IdentityRole> roleManager, BlogContext blogContext) : base(roleManager, blogContext)
        {
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public void OnGet() { }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = "Dữ liệu không hợp lệ";
                return Page();
            }
            var newRole = new IdentityRole(Input.Name);
            var createRole = await _roleManager.CreateAsync(newRole);
            if (createRole.Succeeded)
            {
                StatusMessage = $"Đã tạo thành công role : {Input.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                StatusMessage = $"Tạo role thất bại : {Input.Name} đã tồn tại";
                return Page();
            }
        }
    }
}

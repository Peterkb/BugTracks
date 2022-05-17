using BugTracksV3.Areas.Identity.Data;
using BugTracksV3.Extensions;
using BugTracksV3.Models.ViewModels;
using BugTracksV3.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTracksV3.Controllers
{
    [Authorize]
    public class UserRolesController : Controller
    {
        private readonly IBTRolesService _rolesService;
        private readonly IBTCompanyInfoService _companyInfoService;

        public UserRolesController(IBTRolesService rolesService, IBTCompanyInfoService companyInfoService)
        {
            _rolesService = rolesService;
            _companyInfoService = companyInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            List<ManageUserRolesViewModel> models = new();

            int companyId = User.Identity.GetCompanyId().Value;

            List<ApplicationUser> users = await _companyInfoService.GetAllMembersAsync(companyId);

            foreach (ApplicationUser user in users)
            {
                ManageUserRolesViewModel viewModel = new();
                viewModel.AppUser = user;
                IEnumerable<string> selectedRole = await _rolesService.GetUserRolesAsync(user);
                viewModel.Roles = new MultiSelectList(await _rolesService.GetRolesAsync(), "Name", "Name", selectedRole);

                models.Add(viewModel);
            }
            //Return model to the view
            return View(models);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel member)
        {
            //Get Company Id
            int companyId = User.Identity.GetCompanyId().Value;

            //Instantiate AppUser
            ApplicationUser user = (await _companyInfoService.GetAllMembersAsync(companyId)).FirstOrDefault(u => u.Id == member.AppUser.Id);

            //Get Roles for User
            IEnumerable<string> roles = await _rolesService.GetUserRolesAsync(user);

            //Grab selected role
            string userRole = member.SelectedRoles.FirstOrDefault();
            if (!string.IsNullOrEmpty(userRole))
            {
                //Remove User from role/s
                if (await _rolesService.RemoveUserFromRolesAsync(user, roles))
                {
                    //Add user to new role/s
                    await _rolesService.AddUserToRoleAsync(user, userRole);
                }
            }
            return RedirectToAction(nameof(ManageUserRoles));
        }
    }
}

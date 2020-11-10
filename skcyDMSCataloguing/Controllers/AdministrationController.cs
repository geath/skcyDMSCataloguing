using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using skcyDMSCataloguing.Models;
using skcyDMSCataloguing.ViewModels;

namespace skcyDMSCataloguing.Controllers
{
    [Authorize(Policy ="AdminRolePolicy")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger<AdministrationController> logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<IdentityUser> userManager,                                        
                                        ILogger<AdministrationController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
        }

        #region Claims
        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"UserID: {userId} can't be found";
                return View("NotFound");
            }

            var existingUserClaims = await userManager.GetClaimsAsync(user);
            var model = new UserClaimsViewModel
            {
                UserId = user.Id
            };

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };
                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value=="true"))
                    { userClaim.IsSelected = true;}
                model.Claims.Add(userClaim);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model,string userId)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"UserID: {model.UserId} can't be found";
                return View("NotFound");
            }

            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can't remove claims");
                return View(model);
            }

            result = await userManager.AddClaimsAsync(user,
                        model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));
            
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can't add claims to the user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { id = model.UserId }); 
        }


            #endregion

            #region Roles
            [HttpGet]
        public IActionResult ListOfRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

                [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListOfRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"The requested RoleId: {id} doesn't exist";
                return View("NotFound");
            }

            var model = new EditRolleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditRole(EditRolleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"RoleID : {model.Id} can't be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded) { return RedirectToAction("ListOfRoles"); }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"RoleID : {roleId} can't be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                { userRoleViewModel.IsUserSelected = true; }
                else
                { userRoleViewModel.IsUserSelected = false; }

                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"RoleID : {roleId} can't be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;

                if (model[i].IsUserSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }

                else if (!model[i].IsUserSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else RedirectToAction("EditRole", new { Id = roleId });
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpPost]
        [Authorize(Policy ="DeleteRolePolicy")]
        public async Task<ActionResult> DeleteRole(string id)
        {
            var roletodelete = await roleManager.FindByIdAsync(id);
            if (roletodelete == null)
            {
                ViewBag.ErrorMessage = $"The requested RoleID: {id} doesn't exist";
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await roleManager.DeleteAsync(roletodelete);
                    if (result.Succeeded) { return RedirectToAction("ListOfRoles"); }
                    foreach (var error in result.Errors) { ModelState.AddModelError("", error.Description); }
                    return View("ListOfRoles");
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.ErrorTitle = $"{roletodelete.Name} role is in use";
                    ViewBag.ErrorMessage = $"{roletodelete.Name} can't be deleted because users are assigned to it." +
                        $"If you want to proceed with deletion please remove the users from the role and try again.";
                    logger.LogError($"Exception Occured: {ex}");
                    return View("Error");
                }
            }            
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId) 
        {
            ViewBag.userId = userId;

            var user = await userManager.FindByIdAsync(userId);
            if(user==null)
            {
                ViewBag.ErrorMessage = $"User with Id ={userId} can't be found";
                return View("NotFound");
            }

            var model = new List<RolesAssignedToUserViewModel>();

            foreach(var role in roleManager.Roles)
            {
                var userRolesViewModel = new RolesAssignedToUserViewModel 
                    {
                        RoleId=role.Id,
                        RoleName=role.Name
                    };
                if (await userManager.IsInRoleAsync(user, role.Name))
                     { userRolesViewModel.IsSelected = true; }
                else
                     { userRolesViewModel.IsSelected = false; }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<RolesAssignedToUserViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id ={userId} can't be found";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Roles can't be removed from the specific user");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user, model.Where(m => m.IsSelected).Select(r => r.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Roles can't be added from the specific user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { id = userId });
        }



        #endregion

        #region Users
        [HttpGet]
        public IActionResult ListOfUsers()
        {
           var users= userManager.Users;

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var usertoedit = await userManager.FindByIdAsync(id);

            if (usertoedit == null)
            {
                ViewBag.ErrorMessage = $"The requested UserID: {id} doesn't exist";
                return View("NotFound");
            }

            var userClaims = await userManager.GetClaimsAsync(usertoedit);
            var userRoles = await userManager.GetRolesAsync(usertoedit);

            var model = new EditUserViewModel
            {
                Id = usertoedit.Id,
                UserEmail = usertoedit.Email,
                UserName = usertoedit.UserName,
                Claims = userClaims.Select(c=>c.Type + " : " + c.Value).ToList(),
                Roles= userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var usertoedit = await userManager.FindByIdAsync(model.Id);

            if (usertoedit == null)
            {
                ViewBag.ErrorMessage = $"The requested UserID: {model.Id} doesn't exist";
                return View("NotFound");
            }
            else
            {
                usertoedit.Email = model.UserEmail;
                usertoedit.UserName = model.UserName;
            }
            var result = await userManager.UpdateAsync(usertoedit);

            if (result.Succeeded) { return RedirectToAction("ListOfUsers"); }
            foreach(var error in result.Errors) { ModelState.AddModelError("", error.Description); }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var usertodelete = await userManager.FindByIdAsync(id);
            if (usertodelete == null)
            {
                ViewBag.ErrorMessage = $"The requested UserID: {id} doesn't exist";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(usertodelete);
                if (result.Succeeded) { return RedirectToAction("ListOfUsers"); }
                foreach(var error in result.Errors) { ModelState.AddModelError("", error.Description); }
            }
            return View("ListOfUsers");

        }


        #endregion
    }
}

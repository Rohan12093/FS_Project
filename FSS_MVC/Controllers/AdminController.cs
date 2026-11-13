using FSS_MVC.Data;
using FSS_MVC.ViewModels;
using FSS_MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AdminControlProj.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly UserManager<ApplicationUser> _users;         // Used to manage users.
        private readonly RoleManager<IdentityRole> _roles;         // Used to manage roles.

        public AdminController(UserManager<ApplicationUser> users, RoleManager<IdentityRole> roles)
        {
            _users = users;
            _roles = roles;
        }


        //https://localhost:7218/Admin/Roles
        public IActionResult Roles()
        {
            // Retrieve all roles from the database.
            var roles = _roles.Roles.ToList();

            // Pass the roles to the Roles view.
            return View(roles);
        }
        public async Task<IActionResult> Users()
        {
            // Retrieve all registered users.
            var users = _users.Users.ToList();

            // Create a list to store users with their roles.
            var vms = new List<UserWithRolesVm>();

            // Go through each user.
            foreach (var user in users)
            {
                // Get the roles assigned to this user.
                var roles = await _users.GetRolesAsync(user);

                // Create a ViewModel containing user details.
                var vm = new UserWithRolesVm
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Roles = roles
                };

                // Add the ViewModel to our list.
                vms.Add(vm);


            }
            // Pass the list to the Users view.
            return View(vms);
        }

        // ==========================================
        // PART 3: EDIT A USER
        // ==========================================

        // Display the Edit User page.
        // GET: /Admin/EditUser/id
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            // Find the selected user.
            var user = await _users.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Retrieve the names of all available roles.
            var allRoles = _roles.Roles
                .Select(r => r.Name!)
                .ToList();

            // Retrieve the roles currently assigned to the user.
            var userRoles = await _users.GetRolesAsync(user);

            // Create the ViewModel.
            var vm = new EditUserVM
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,

                // All available roles.
                AvailableRoles = allRoles,

                // Roles currently assigned to the user.
                SelectedRoles = userRoles.ToList()
            };

            // Display the Edit User page.
            return View(vm);
        }



        public async Task<IActionResult> EditUser(EditUserVM vm)
        {
            // 1) Find the user or 404
            var user = await _users.FindByIdAsync(vm.Id);
            if (user == null) return NotFound();

            // 2) Update basic fields
            user.Email = vm.Email;
            user.UserName = vm.Email;     // keep username aligned with email
            user.FirstName = vm.FirstName;
            await _users.UpdateAsync(user);

            // 3) Replace roles with the selected ones (simplest approach)
            var current = await _users.GetRolesAsync(user);           // current roles
            if (current.Count > 0)
                await _users.RemoveFromRolesAsync(user, current);     // clear all

            var selected = vm.SelectedRoles ?? new List<string>();    // handle null safely
            if (selected.Count > 0)
                await _users.AddToRolesAsync(user, selected);         // set final roles

            // 4) Back to list
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _users.FindByIdAsync(id);
            if (user != null) await _users.DeleteAsync(user);
            return RedirectToAction(nameof(Users));
        }
    }
}



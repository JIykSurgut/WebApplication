using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Models;
using System.Net;

namespace ZDL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : Controller
    {
        public UsersAdminController()
        {
        }
        public UserManager appUserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UserManager>(); }

        }
        public RoleManager appRoleManager
        {
            get { return HttpContext.GetOwinContext().Get<RoleManager>(); }
        }

        [HttpGet] //OK
        public async Task<ActionResult> Index()
        {
            return View(await appUserManager.GetUsersAsync());
        }

        [HttpGet] //OK
        public async Task<ActionResult> Details(int id)
        {
            User user = await appUserManager.FindByIdAsync(id);
            if (user != null)
            {
                ViewBag.RoleNames = await appUserManager.GetRolesAsync(user.Id);
                return View(user);
            }
            return RedirectToAction("Index");
        }

        [HttpGet] //OK
        public async Task<ActionResult> Create()
        {
            ViewBag.Roles = new SelectList(await appRoleManager.GetRolesAsync(), "Name", "Name");
            return View();
        }
        [HttpPost] //OK!
        public async Task<ActionResult> Create(User user, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                IdentityResult userInsertResult = await appUserManager.CreateAsync(user, user.PasswordHash);

                if (userInsertResult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        User userInsert = await appUserManager.FindByNameAsync(user.UserName);

                        IdentityResult addToRoleResult = await appUserManager.AddToRolesAsync(userInsert.Id, selectedRoles);
                        if (!addToRoleResult.Succeeded)
                        {
                            ModelState.AddModelError("", addToRoleResult.Errors.First());
                            ViewBag.RoleId = new SelectList(await appRoleManager.GetRolesAsync(), "Name", "Name");
                            return View(); //? передача user и ролей
                        }
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", userInsertResult.Errors.First());
                    ViewBag.Roles = new SelectList(await appRoleManager.GetRolesAsync(), "Name", "Name");
                    return View(); //? передача user и ролей

                }
            }
            ViewBag.Roles = new SelectList(await appRoleManager.GetRolesAsync(), "Name", "Name");
            return View(); //? передача user и ролей
        }

        [HttpGet] //OK
        public async Task<ActionResult> Edit(int id)
        {
            User user = await appUserManager.FindByIdAsync(id);
            if (user != null)
            {
                IList<string> rolesNameUser = await appUserManager.GetRolesAsync(user.Id);
                IList<Role> rolesAll = await RoleManager.GetRolesAsync();

                List<SelectListItem> roles = new List<SelectListItem>();
                foreach (Role role in rolesAll)
                {
                    roles.Add(new SelectListItem()
                    {
                        Selected = (rolesNameUser.Contains(role.Name)),
                        Text = role.Name,
                        Value = Convert.ToString(role.Id)
                    });
                }

                ViewBag.Roles = roles;
                return View(user);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult> Edit(User user, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                User userValid = await appUserManager.FindByIdAsync(user.Id);
                if (userValid != null)
                {
                    user.PasswordHash = userValid.PasswordHash;
                    IdentityResult identity = await appUserManager.UpdateAsync(user);

                    List<Role> roles = await RoleManager.GetRolesAsync();


                    foreach (Role role in roles)
                    {
                        if (selectedRole != null && selectedRole.Contains(role.Name))
                        {
                            await appUserManager.AddToRoleAsync(userValid.Id, role.Name);
                        }
                        else
                        {
                            await appUserManager.RemoveFromRoleAsync(userValid.Id, role.Name);
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Roles = new SelectList(await appRoleManager.GetRolesAsync(), "Name", "Name");
            return View(); //передать user и роли
        }

        [HttpGet] //OK
        public async Task<ActionResult> Delete(int id)
        {
            User user = await appUserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("Delete")] //OK!
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //Необходимо реализовать удаление всех ролей у удаленного пользователя
            User user = await appUserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult identityResult = await appUserManager.DeleteAsync(user);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", identityResult.Errors.First());
                return View(user);
            }
            return RedirectToAction("Index");
        }
    }
}
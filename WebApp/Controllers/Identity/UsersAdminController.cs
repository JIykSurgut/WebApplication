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
        public UserManager<User, int> UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UserManager<User, int>>(); }

        }
        public RoleManager<Role, int> RoleManager
        {
            get { return HttpContext.GetOwinContext().Get<RoleManager<Role, int>>(); }
        }

        [HttpGet] //OK
        public async Task<ActionResult> Index()
        {
            //return View(await UserManager.GetUsersAsync());
            return null;
        }

        [HttpGet] //OK
        public async Task<ActionResult> Details(int id)
        {
            User user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);
                return View(user);
            }
            return RedirectToAction("Index");
        }

        [HttpGet] //OK
        public async Task<ActionResult> Create()
        {
            //ViewBag.Roles = new SelectList(await RoleManager.GetRolesAsync(), "Name", "Name");
            return View();
        }
        [HttpPost] //OK!
        public async Task<ActionResult> Create(User user, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                IdentityResult userInsertResult = await UserManager.CreateAsync(user, user.PasswordHash);

                if (userInsertResult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        User userInsert = await UserManager.FindByNameAsync(user.UserName);

                        IdentityResult addToRoleResult = await UserManager.AddToRolesAsync(userInsert.Id, selectedRoles);
                        if (!addToRoleResult.Succeeded)
                        {
                            ModelState.AddModelError("", addToRoleResult.Errors.First());
                            //ViewBag.RoleId = new SelectList(await RoleManager.GetRolesAsync(), "Name", "Name");
                            return View(); //? передача user и ролей
                        }
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", userInsertResult.Errors.First());
                    //ViewBag.Roles = new SelectList(await appRoleManager.GetRolesAsync(), "Name", "Name");
                    return View(); //? передача user и ролей

                }
            }
            //ViewBag.Roles = new SelectList(await appRoleManager.GetRolesAsync(), "Name", "Name");
            return View(); //? передача user и ролей
        }

        [HttpGet] //OK
        public async Task<ActionResult> Edit(int id)
        {
            //User user = await UserManager.FindByIdAsync(id);
            //if (user != null)
            //{
            //    IList<string> rolesNameUser = await UserManager.GetRolesAsync(user.Id);
            //    //IList<Role> rolesAll = await RoleManager.GetRolesAsync();

            //    List<SelectListItem> roles = new List<SelectListItem>();
            //    foreach (Role role in rolesAll)
            //    {
            //        roles.Add(new SelectListItem()
            //        {
            //            Selected = (rolesNameUser.Contains(role.Name)),
            //            Text = role.Name,
            //            Value = Convert.ToString(role.Id)
            //        });
            //    }

            //    ViewBag.Roles = roles;
            //    return View(user);
            //}
            //return RedirectToAction("Index");
            return null;
        }
        [HttpPost]
        public async Task<ActionResult> Edit(User user, params string[] selectedRole)
        {
            //if (ModelState.IsValid)
            //{
            //    User userValid = await UserManager.FindByIdAsync(user.Id);
            //    if (userValid != null)
            //    {
            //        user.PasswordHash = userValid.PasswordHash;
            //        IdentityResult identity = await UserManager.UpdateAsync(user);

            //        List<Role> roles = await RoleManager.GetRolesAsync();


            //        foreach (Role role in roles)
            //        {
            //            if (selectedRole != null && selectedRole.Contains(role.Name))
            //            {
            //                await appUserManager.AddToRoleAsync(userValid.Id, role.Name);
            //            }
            //            else
            //            {
            //                await appUserManager.RemoveFromRoleAsync(userValid.Id, role.Name);
            //            }
            //        }
            //        return RedirectToAction("Index");
            //    }
            //}
            //ViewBag.Roles = new SelectList(await appRoleManager.GetRolesAsync(), "Name", "Name");
            //return View(); //передать user и роли
            return null;
        }

        [HttpGet] //OK
        public async Task<ActionResult> Delete(int id)
        {
            User user = await UserManager.FindByIdAsync(id);
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
            User user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult identityResult = await UserManager.DeleteAsync(user);
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
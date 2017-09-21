using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

using Models;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNet.Identity;

namespace ZDL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesAdminController : Controller
    {
        public RolesAdminController()
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

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            //return View(await RoleManager.GetRolesAsync());
            return null;
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            Role role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                List<User> users = new List<User>();
                //foreach (User user in await UserManager.GetUsersAsync())
                //{
                //    if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                //    {
                //        users.Add(user);
                //    }
                //}
                ViewBag.Users = users;
                ViewBag.UserCount = users.Count();
                return View(role);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Role role)
        {
            if (ModelState.IsValid)
            {
                IdentityResult identityResult = await RoleManager.CreateAsync(role);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", identityResult.Errors.First());
                return View(role);
            }
            return View(role);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            Role role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                return View(role);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult> Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                Role roleValid = await RoleManager.FindByIdAsync(role.Id);
                if (roleValid != null)
                {
                    IdentityResult identityResult = await RoleManager.UpdateAsync(role);
                    if (identityResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", identityResult.Errors.First());
                    return View(role);
                }
                ModelState.AddModelError("", "Роль не найдена");
                return View(role);
            }
            return View(role);
        }

        [HttpGet] //!
        public async Task<ActionResult> Delete(int id)
        {
            Role role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                return View(role);
            }
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //Удалить у пользователей удаленную роль
            Role role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult identityResult = await RoleManager.DeleteAsync(role);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", identityResult.Errors.First());
                return View(role);
            }
            return RedirectToAction("Index");
        }
    }
}
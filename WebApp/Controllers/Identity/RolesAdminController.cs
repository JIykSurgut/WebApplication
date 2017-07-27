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
        public AppUserManager appUserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
        }
        public AppRoleManager appRoleManager
        {
            get { return HttpContext.GetOwinContext().Get<AppRoleManager>(); }
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await appRoleManager.GetRolesAsync());
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            AppRole role = await appRoleManager.FindByIdAsync(id);
            if (role != null)
            {
                List<AppUser> users = new List<AppUser>();
                foreach (AppUser user in await appUserManager.GetUsersAsync())
                {
                    if (await appUserManager.IsInRoleAsync(user.Id, role.Name))
                    {
                        users.Add(user);
                    }
                }
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
        public async Task<ActionResult> Create(AppRole role)
        {
            if (ModelState.IsValid)
            {
                IdentityResult identityResult = await appRoleManager.CreateAsync(role);
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
            AppRole role = await appRoleManager.FindByIdAsync(id);
            if (role != null)
            {
                return View(role);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult> Edit(AppRole role)
        {
            if (ModelState.IsValid)
            {
                AppRole roleValid = await appRoleManager.FindByIdAsync(role.Id);
                if (roleValid != null)
                {
                    IdentityResult identityResult = await appRoleManager.UpdateAsync(role);
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
            AppRole role = await appRoleManager.FindByIdAsync(id);
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
            AppRole role = await appRoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult identityResult = await appRoleManager.DeleteAsync(role);
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
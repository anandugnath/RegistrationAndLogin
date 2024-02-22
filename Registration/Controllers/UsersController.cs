using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Registration;
using Registration.Models;

namespace Registration.Controllers
{
    public class UsersController : Controller
    {
        private readonly CoreDBContext _context;

        public UsersController(CoreDBContext context)
        {

            _context = context;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                
                return RedirectToAction("Login", "Users");
            }

            return View(await _context.Users.ToListAsync());
              //return _context.Users != null ? 
              //            View(await _context.Users.ToListAsync()) :
              //            Problem("Entity set 'CoreDBContext.Users'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int id=0)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoginCheck( [FromBody]Users obj)
        {
          

            if (obj.EmailID!=null && obj.Password!=null)
            {
                var a=_context.Users.Where(t=> t.EmailID==obj.EmailID && t.Password==obj.Password).FirstOrDefault();
            if(a!=null)
                {
                    HttpContext.Session.SetInt32("UserID", a.UserID);
                    HttpContext.Session.SetString("EmailID", a.EmailID);

                    string redirectUrl = Url.Action("Index", "Home");

                    return Json(new { isValid = true, redirectUrl });
                   
                }

            }
            return Json(new { isValid = false });
        }
        public async Task<IActionResult> AddMultiple(int UserID)
        {
            List<SelectListItem> UserType = new List<SelectListItem>()
            {
        new SelectListItem() {Text="Admin", Value="Admin"},
        new SelectListItem() { Text="User", Value="User"},

             };

           List< Users> entities = new List<Users>();
            ViewBag.UserTypes = UserType;
            if (UserID == 0)
                return View(entities);
            else
            {
                var UserModel = await _context.Users.FindAsync(UserID);
                if (UserModel == null)
                {
                    return NotFound();
                }
                return View(UserModel);
            }

        }

        public async Task<IActionResult> AddMultipleNew(int UserID)
        {
            List<SelectListItem> UserType = new List<SelectListItem>()
            {
        new SelectListItem() {Text="Admin", Value="Admin"},
        new SelectListItem() { Text="User", Value="User"},

             };

            List<Users> entities = new List<Users>();
            ViewBag.UserTypes = UserType;
            if (UserID == 0)
                return View(entities);
            else
            {
                var UserModel = await _context.Users.FindAsync(UserID);
                if (UserModel == null)
                {
                    return NotFound();
                }
                return View(UserModel);
            }

        }


        public JsonResult InsertBulkUsers([FromBody]List<Users> Users)
        {
            int Cnt = 0;
            foreach(Users Usr in Users)
            {
                Cnt++;
                Usr.AddedDate = DateTime.Now;


                _context.Add(Usr);
                _context.SaveChanges();

            }
         var id=   _context.SaveChanges();
            return Json(Cnt);
        }
        public async Task< IActionResult> Add(int UserID)
        {
            List<SelectListItem> UserType = new List<SelectListItem>()
    {
        new SelectListItem() {Text="Admin", Value="Admin"},
        new SelectListItem() { Text="User", Value="User"},

    };

            ViewBag.UserTypes = UserType;
            if (UserID== 0)
                return View(new Users());
            else
            {
                var UserModel = await _context.Users.FindAsync(UserID);
                if (UserModel == null)
                {
                    return NotFound();
                }
                return View(UserModel);
            }
            
        }

        
        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int UserID, [Bind("UserID,EmailID,Password,RegistrationType,IsActive,EmailVerified,AddedDate")] Users users)
        {
            if(users.RegistrationType==null)
            {
                users.RegistrationType = "B";
            }
            if (!ModelState.IsValid)
            {
                
                    if (UserID != 0)
                    {

                        var a = _context.Users.Find(UserID);
                        a.RegistrationType = users.RegistrationType;
                        a.EmailVerified = users.EmailVerified;
                        a.EmailID = users.EmailID;
                        a.IsActive = users.IsActive;

                        _context.Update(a);

                        await _context.SaveChangesAsync();
                        //  return RedirectToAction(nameof(Index));
                        var lst = _context.Users;
                        return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Index", lst.ToList()) });

                    }
                    else
                    {
                        users.UserID = 0;
                        users.AddedDate = DateTime.Now;
                        _context.Add(users);
                        await _context.SaveChangesAsync();
                        //  return RedirectToAction(nameof(Index));
                        var lst = _context.Users;
                        return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Index", lst.ToList()) });

                    }

               
            }
           
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Add", users) });

        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,EmailID,Password,RegistrationType,IsActive,EmailVerified,AddedDate")] Users users)
        {
            if (id != users.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'CoreDBContext.Users'  is null.");
            }
            var users = await _context.Users.FindAsync(id);
            if (users != null)
            {
                _context.Users.Remove(users);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
          return (_context.Users?.Any(e => e.UserID == id)).GetValueOrDefault();
        }
    }
}

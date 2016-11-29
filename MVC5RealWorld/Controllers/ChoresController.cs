using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5RealWorld.Models.DB;
using MVC5RealWorld.Models.EntityManager;
using MVC5RealWorld.Models.ViewModel;
using MVC5RealWorld.Security;

namespace MVC5RealWorld.Controllers
{
    public class ChoresController : Controller
    {
        private DB_A11531_francistestEntities db = new DB_A11531_francistestEntities();

        // GET: Chores
        [AuthorizeRoles("Admin", "Parents")]
        public ActionResult Index()
        {
            return View(db.Chores.ToList());
        }

        [AuthorizeRoles("Toddler", "Child", "Teen", "Young Adult", "Adult")]
        public ActionResult IndexChild()
        {
            return View(db.Chores.ToList());
        }

        // GET: Chores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chore chore = db.Chores.Find(id);
            if (chore == null)
            {
                return HttpNotFound();
            }
            return View(chore);
        }

        // GET: Chores/Create
        [AuthorizeRoles("Admin","Parents")]
        public ActionResult Create()
        {
            SelectList profiles = new SelectList(GetAllUserProfiles());
            ViewBag.Users = profiles;
            
            return View();
        }

        // POST: Chores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AuthorizeRoles("Admin", "Parents")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChoreID,Title,Description,BronzeValue,SilverValue,GoldValue,AssignedTo,IsAssigned,IsCompleted,IsConfirmed,MaturityLevel,StartDate,DeadlineDate,NeedExtension,RequestedExtensionDeadline,Multiplier")] Chore chore)
        {
           
            if (ModelState.IsValid)
            {
                db.Chores.Add(chore);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chore);
        }

        // GET: Chores/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chore chore = db.Chores.Find(id);
            if (chore == null)
            {
                return HttpNotFound();
            }
            return View(chore);
        }

        // POST: Chores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChoreID,Title,Description,BronzeValue,SilverValue,GoldValue,AssignedTo,IsAssigned,IsCompleted,IsConfirmed,MaturityLevel,StartDate,DeadlineDate,NeedExtension,RequestedExtensionDeadline,Multiplier")] Chore chore)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chore).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chore);
        }

        // GET: Chores/Delete/5
        [AuthorizeRoles("Admin", "Parents")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chore chore = db.Chores.Find(id);
            if (chore == null)
            {
                return HttpNotFound();
            }
            return View(chore);
        }

        // POST: Chores/Delete/5
        [AuthorizeRoles("Admin", "Parents")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chore chore = db.Chores.Find(id);
            db.Chores.Remove(chore);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public List<UserProfileView> GetAllUserProfiles()
        {
            List<UserProfileView> profiles = new List<UserProfileView>();
            using (DB_A11531_francistestEntities db = new DB_A11531_francistestEntities())
            {
                UserProfileView UPV;
                var users = db.SYSUsers.ToList();

                foreach (SYSUser u in db.SYSUsers)
                {
                    UPV = new UserProfileView();
                    UPV.SYSUserID = u.SYSUserID;
                    UPV.LoginName = u.LoginName;
                    UPV.Password = u.PasswordEncryptedText;

                    var SUP = db.SYSUserProfiles.Find(u.SYSUserID);
                    if (SUP != null)
                    {
                        UPV.FirstName = SUP.FirstName;
                        UPV.LastName = SUP.LastName;
                        UPV.Gender = SUP.Gender;
                    }

                    var SUR = db.SYSUserRoles.Where(o => o.SYSUserID.Equals(u.SYSUserID));
                    if (SUR.Any())
                    {
                        var userRole = SUR.FirstOrDefault();
                        UPV.LOOKUPRoleID = userRole.LOOKUPRoleID;
                        UPV.RoleName = userRole.LOOKUPRole.RoleName;
                        UPV.IsRoleActive = userRole.IsActive;
                    }

                    profiles.Add(UPV);
                }
            }

            return profiles;
        }
    }
}

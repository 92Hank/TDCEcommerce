using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheDogsCorner.DAL;

namespace TheDogsCorner.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [AuthorizationFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        [AuthorizationFilter]
        public ActionResult UserCreate()
        {
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public JsonResult CheckLogin(string username, string password)
        {
            ElGiganto20Entities1 db = new ElGiganto20Entities1();
            string md5StringPassword = AppHelper.GetMdHash(password);
            var dataItem = db.User.Where(x => x.Username == username && x.Password == md5StringPassword).SingleOrDefault();
            bool isLogged = true;
            if (dataItem != null)
            {
                Session["Username"] = dataItem.Username;
                Session["Role"] = dataItem.Role;
                isLogged = true;
            }
            else
            {
                isLogged = false;
            }
            return Json(isLogged, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveUser(User user)
        {
            ElGiganto20Entities1 db = new ElGiganto20Entities1();
            bool isSuccess = true;

            if (user.UserId > 0)
            {
                db.Entry(user).State = EntityState.Modified;
            }
            else
            {
                user.Status = 1;
                user.Password = AppHelper.GetMdHash(user.Password);
                db.User.Add(user);
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                isSuccess = false;
            }

            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllUser()
        {
            ElGiganto20Entities1 db = new ElGiganto20Entities1();
            var dataList = db.User.Where(x => x.Status == 1).ToList();
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session["Username"] = null;
            Session["Role"] = null;
            return RedirectToAction("Login");
        }
    }
}
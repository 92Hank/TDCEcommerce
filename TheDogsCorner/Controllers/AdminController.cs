using TheDogsCorner.DAL;
using TheDogsCorner.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TheDogsCorner.Models;
using TheDogsCorner.Helper;
using System.Data.Entity;

namespace TheDogsCorner.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = _unitOfWork.GetRepositoryInstance<Kategori>().GetAllRecords();
            foreach (var item in cat)
            {
                list.Add(new SelectListItem { Value = item.KategoriId.ToString(), Text = item.KategoriNamn });
            }
            return list;
        }
        [AuthorizationFilter]
        public ActionResult Dashboard()
        {
            return View();
        }

        [AuthorizationFilter]
        public ActionResult Categories()
        {
            List<Kategori> allcategories = _unitOfWork.GetRepositoryInstance<Kategori>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false).ToList();
            return View(allcategories);
        }
        [AuthorizationFilter]
        public ActionResult AddCategory()
        {
            return UpdateCategory(0);
        }
        [AuthorizationFilter]
        public ActionResult UpdateCategory(int categoryId)
        {
            KategoriBeskrivning cd;
            if (categoryId != null)
            {
                cd = JsonConvert.DeserializeObject<KategoriBeskrivning>(JsonConvert.SerializeObject(_unitOfWork.GetRepositoryInstance<Kategori>().GetFirstorDefault(categoryId)));
            }
            else
            {
                cd = new KategoriBeskrivning();
            }
            return View("UpdateCategory", cd);
        }
        [AuthorizationFilter]
        public ActionResult CategoryEdit(int catId)
        {
            return View(_unitOfWork.GetRepositoryInstance<Kategori>().GetFirstorDefault(catId));
        }
        [HttpPost]
        public ActionResult CategoryEdit(Kategori tbl)
        {
            _unitOfWork.GetRepositoryInstance<Kategori>().Update(tbl);
            return RedirectToAction("Categories");
        }
        [AuthorizationFilter]
        public ActionResult Product()
        {
            return View(_unitOfWork.GetRepositoryInstance<Produkt>().GetProduct());
        }
        [AuthorizationFilter]
        public ActionResult ProductEdit(int productId)
        {
            ViewBag.CategoryList = GetCategory();
            return View(_unitOfWork.GetRepositoryInstance<Produkt>().GetFirstorDefault(productId));
        }
        [HttpPost]
        public ActionResult ProductEdit(Produkt tbl, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);

                file.SaveAs(path);
            }
            tbl.ProductImage = file != null ? pic : tbl.ProductImage;
            tbl.ModifieradDatum = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Produkt>().Update(tbl);
            return RedirectToAction("Product");
        }
        [AuthorizationFilter]
        public ActionResult ProductAdd()
        {
            ViewBag.CategoryList = GetCategory();
            return View();
        }

        [AuthorizationFilter]
        public ActionResult UserCreate()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveUser(Users user)
        {
            TheDogsCornerEntities db = new TheDogsCornerEntities();
            bool isSuccess = true;

            if (user.UsersId > 0)
            {
                db.Entry(user).State = EntityState.Modified;
            }
            else
            {
                user.Status = 1;
                user.Password = AppHelper.GetMdHash(user.Password);
                db.Users.Add(user);
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
            TheDogsCornerEntities db = new TheDogsCornerEntities();
            var dataList = db.Users.Where(x => x.Status == 1).ToList();
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ProductAdd(Produkt tbl, HttpPostedFileBase file)
        {
            string pic=null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);

                file.SaveAs(path);
            }
            tbl.ProductImage = pic;
            tbl.SkapadDatum = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Produkt>().Add(tbl);
            return RedirectToAction("Product");
        }
    }
}
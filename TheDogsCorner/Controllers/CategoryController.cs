using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheDogsCorner.DAL;
using TheDogsCorner.Models.Home;
using TheDogsCorner.Repository;
using PagedList;
using System.Web.UI;

namespace TheDogsCorner.Controllers
{
    public class CategoryController : Controller
    {
        TheDogsCornerEntities db = new TheDogsCornerEntities();
        HomeIndexModel home = new HomeIndexModel();
        GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

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
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult CategoryPartial(string search, int? page)
        {
            //var categoryList = db.Kategoris.OrderBy(x => x.KategoriNamn).ToList();
            HomeIndexModel model = new HomeIndexModel();
            return PartialView(model.CategoryModel(search, 20, page));
        }
    }
}
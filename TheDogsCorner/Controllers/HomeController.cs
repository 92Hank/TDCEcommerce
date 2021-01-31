using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheDogsCorner.DAL;
using TheDogsCorner.Models.Home;
using TheDogsCorner.Repository;
using Newtonsoft.Json;
using TheDogsCorner.Helper;
using System.Data.Entity;

namespace TheDogsCorner.Controllers
{
    public class HomeController : Controller
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        TheDogsCornerEntities ctx = new TheDogsCornerEntities();
        private string strCart = "cart";

        public ActionResult Index(string search, int? page)
        {
            HomeIndexModel model = new HomeIndexModel();
            return View(model.CreateModel(search, 8, page));
        }


        public ActionResult Login()
        {
            ViewBag.Message = "Your application Login page.";

            return View();
        }

        public JsonResult CheckLogin(string username, string password)
        {
            TheDogsCornerEntities db = new TheDogsCornerEntities();
            string md5StringPassword = AppHelper.GetMdHash(password);
            var dataItem = db.Users.Where(x => x.Username == username && x.Password == md5StringPassword).SingleOrDefault();
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

        public ActionResult Register()
        {
            ViewBag.Message = "Your application Register page.";

            return View();
        }

        [AuthorizationFilter]
        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult CheckoutDetails()
        {
            return View();
        }

        public ActionResult DecreaseQty(int productId)
        {
            if (Session[strCart] != null)
            {
                List<Item> cart = (List<Item>)Session[strCart];
                var product = ctx.Produkt.Find(productId);
                foreach (var item in cart)
                {
                    if (item.Produkt.ProduktId == productId)
                    {
                        int prevQty = item.Kvantitet;
                        if (prevQty > 0)
                        {
                            cart.Remove(item);
                            cart.Add(new Item()
                            {
                                Produkt = product,
                                Kvantitet = prevQty - 1
                            });
                        }
                        break;
                    }
                }
                Session[strCart] = cart;
            }
            return Redirect("Checkout");
        }

        public ActionResult AddToCart(int productId, string url)
        {
            if (Session[strCart] == null)
            {
                List<Item> cart = new List<Item>();
                var product = ctx.Produkt.Find(productId);
                cart.Add(new Item()
                {
                    Produkt = product,
                    Kvantitet = 1
                });
                Session[strCart] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session[strCart];
                var count = cart.Count();
                var product = ctx.Produkt.Find(productId);
                for (int i = 0; i < count; i++)
                {
                    if (cart[i].Produkt.ProduktId == productId)
                    {
                        int prevQty = cart[i].Kvantitet;
                        cart.Remove(cart[i]);
                        cart.Add(new Item()
                        {
                            Produkt = product,
                            Kvantitet = prevQty + 1
                        });
                        break;
                    }
                    else
                    {
                        var prd = cart.Where(x => x.Produkt.ProduktId == productId).SingleOrDefault();
                        if (prd == null)
                        {
                            cart.Add(new Item()
                            {
                                Produkt = product,
                                Kvantitet = 1
                            });
                        }
                    }
                }
                Session[strCart] = cart;
            }
            return Redirect("Index");
        }

        public ActionResult RemoveFromCart(int productId)
        {
            List<Item> cart = (List<Item>)Session[strCart];
            foreach (var item in cart)
            {
                if (item.Produkt.ProduktId == productId)
                {
                    cart.Remove(item);
                    break;
                }
            }

            Session[strCart] = cart;
            return Redirect("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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

        public ActionResult Logout()
        {
            Session["Username"] = null;
            Session["Role"] = null;
            return RedirectToAction("Index");
        }
    }
}

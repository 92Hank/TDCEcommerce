using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheDogsCorner.DAL;
using TheDogsCorner.Repository;
using TheDogsCorner.Models.Home;
using Newtonsoft.Json;
using Item = TheDogsCorner.Models.Home.Item;

namespace TheDogsCorner.Controllers
{
    public class PaymentController : Controller
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        TheDogsCornerEntities ctx = new TheDogsCornerEntities();
        private string strCart = "cart";
        
        public ActionResult FakePayment(FormCollection frc)
        {
            if (Session[strCart] != null)
            {
                List<Models.Home.Item> cart = (List<Models.Home.Item>)Session[strCart];
                //var product = ctx.Produkts.Find(productId);

                var order = new TheDogsCorner.DAL.Orders()
                {
                    CustomerName = frc["cusName"],
                    CustomerPhone = frc["cusPhone"],
                    CustomerEmail = frc["cusEmail"],
                    CustomerAddress = frc["cusAddress"],
                    OrderDate = DateTime.Now,
                    PaymentType = "Cash",
                    Status = "Processing"
                };
                ctx.Orders.Add(order);
                ctx.SaveChanges();

                //2. Save the order detail into Order Detail table
                foreach (Item item in cart)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrdersId = order.OrdersId,
                        ProduktId = item.Produkt.ProduktId,
                        Quantity = item.Kvantitet,
                        Pris = item.Kvantitet * Convert.ToDouble(item.Produkt.Pris)
                    };
                    ctx.OrderDetail.Add(orderDetail);
                    ctx.SaveChanges();


                    Produkt produkt = new Produkt()
                    {
                        ProduktId = orderDetail.ProduktId,
                        ProduktNamn = item.Produkt.ProduktNamn,
                        KategoriId = item.Produkt.KategoriId,
                        UsersId = item.Produkt.UsersId,
                        IsActive = item.Produkt.IsActive,
                        IsDelete = item.Produkt.IsDelete,
                        SkapadDatum = item.Produkt.SkapadDatum,
                        ModifieradDatum = item.Produkt.ModifieradDatum,
                        Beskrivning = item.Produkt.Beskrivning,
                        ProductImage = item.Produkt.ProductImage,
                        IsFeatured = item.Produkt.IsFeatured,
                        Kvantitet = item.Produkt.Kvantitet - item.Kvantitet,
                        Pris = item.Produkt.Pris,
                    };
                    
                    _unitOfWork.GetRepositoryInstance<Produkt>().Update(produkt);
                    ctx.SaveChanges();
                }
            }
            //3. Remove shopping cart session
            Session.Remove(strCart);
            return View("SuccessView");
        }



        // GET: Payment
        /*public ActionResult PaymentWithPapal()
        {

            APIContext apicontext = PaypalConfiguration.GetAPIContext();
            try
            {

                string PayerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(PayerId) && PayerId != null)
                {
                    string baseURi = Request.Url.Scheme + "://" + Request.Url.Authority +
                                     "/PaymentWithPapal/PaymentWithPapal?";

                    var Guid = Convert.ToString((new Random()).Next(100000000));
                    var createPayment = this.CreatePayment(apicontext, baseURi + "guid=" + Guid);

                    var links = createPayment.links.GetEnumerator();
                    string paypalRedirectURL = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectURL = lnk.href;
                        }


                    }
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPaymnt = ExecutePayment(apicontext, PayerId, Session[guid] as string);


                    if (executedPaymnt.ToString().ToLower() != "approved")
                    {
                        return View("FailureView");
                    }

                }
            }
            catch (Exception)
            {
                return View("FailureView");


                //throw;
            }
            return View("SuccessView");

        }

        private object ExecutePayment(APIContext apicontext, string payerId, string PaymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = PaymentId };
            return this.payment.Execute(apicontext, paymentExecution);
        }

        private PayPal.Api.Payment payment;

        private Payment CreatePayment(APIContext apiContext, string redirectURl)
        {

            var ItemLIst = new ItemList() { items = new List<Item>() };

            if (Session["Cart"] != "")
            {
                List<Models.Home.Item> cart = (List<Models.Home.Item>)(Session["Cart"]);
                foreach (var item in cart)
                {
                    ItemLIst.items.Add(new Item()
                    {
                        name = item.Produkt.ProduktNamn.ToString(),
                        currency = "TK",
                        price = item.Produkt.Pris.ToString(),
                        quantity = item.Produkt.Kvantitet.ToString(),
                        sku = "sku"
                    });


                }


                var payer = new Payer() { payment_method = "paypal" };

                var redirUrl = new RedirectUrls()
                {
                    cancel_url = redirectURl + "&Cancel=true",
                    return_url = redirectURl
                };

                var details = new Details()
                {
                    tax = "1",
                    shipping = "1",
                    subtotal = "1"
                };

                var amount = new Amount()
                {
                    currency = "USD",

                    total = Session["SesTotal"].ToString(),
                    details = details
                };

                var transactionList = new List<Transaction>();
                transactionList.Add(new Transaction()
                {
                    description = "Transaction Description",
                    invoice_number = "#100000",
                    amount = amount,
                    item_list = ItemLIst

                });

                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrl
                };
            }



            return this.payment.Create(apiContext);

        }*/
    }
}
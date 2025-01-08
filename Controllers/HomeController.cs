using HelmonyCornDog.Data;
using HelmonyCornDog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using ZarinpalSandbox;

namespace HelmonyCornDog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CornDogContext _context;


        public HomeController(ILogger<HomeController> logger, CornDogContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(bool showModal = false)
        {
            var products = _context.Products.Include(i => i.Item).ToList();

            // for image formats
            foreach (var product in products)
            {
                product.ImagePath = GetImagePath(product.Id);
            }

            // for modal
            ViewData["ShowModal"] = showModal;
            if (TempData.ContainsKey("Name"))
            {
                ViewData["Name"] = TempData["Name"];
            }
            return View(products);
        }
        private string GetImagePath(int productId)
        {
            var imageExtensions = new[] { "jpg", "png", "jpeg", "gif" };
            foreach (var ext in imageExtensions)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", $"{productId}.{ext}");
                if (System.IO.File.Exists(path))
                {
                    return $"/images/{productId}.{ext}";
                }
            }
            return null; // Fallback if no image is found
        }

        public IActionResult Detail(int id)
        {
            var product = _context.Products.Include(s => s.Specifications).Include(i => i.Item)
                .SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ImagePath"] = GetImagePath(product.Id);
            var Categories = _context.Products.Where(p => p.Id == id).SelectMany(c => c.CategoryToProducts)
                .Select(ca => ca.Category).ToList();

            var VM = new DetailsVeiwModel()
            {
                Categories = Categories,
                Product = product
            };

            return View(VM);
        }

        [Authorize]
        public IActionResult AddToCart(int itemid)
        {
            var product = _context.Products.Include(s => s.Specifications).Include(i => i.Item).SingleOrDefault(p => p.ItemId == itemid);

            if (product != null)
            {
                int Userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
                var order = _context.Orders.FirstOrDefault(o => o.UserId == Userid && !o.IsFinally);

                if (order != null)
                {
                    var orderDetail =
                        _context.OrderDetails.FirstOrDefault(o =>
                            o.OrderId == order.OrderID && o.ProductId == product.Id);
                    if (orderDetail != null)
                    {
                        orderDetail.Quantity += 1;
                    }
                    else
                    {
                        _context.OrderDetails.Add(new OrderDetail()
                        {
                            OrderId = order.OrderID,
                            ProductId = product.Id,
                            Price = product.Item.Price,
                            Quantity = 1


                        });
                    }

                }
                else
                {
                    order =
                        new Order()
                        {
                            IsFinally = false,
                            CreatDate = DateTime.Now,
                            UserId = Userid
                        };
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    _context.OrderDetails.Add(new OrderDetail()
                    {
                        OrderId = order.OrderID,
                        ProductId = product.Id,
                        Price = product.Item.Price,
                        Quantity = 1


                    });
                }

                _context.SaveChanges();
            }

            return RedirectToAction("ShowCart");
        }

        [Authorize]
        public IActionResult RemoveFromCart(int DetailId)
        {
            var orderDetail = _context.OrderDetails.Find(DetailId);
            if (orderDetail?.Quantity <= 1)
            {
                _context.Remove(orderDetail);
                _context.SaveChanges();
            }
            else
            {
                orderDetail.Quantity -= 1;
                _context.SaveChanges();
            }
            return RedirectToAction("ShowCart");
        }

        [Authorize]
        public IActionResult ShowCart()
        {
            int Userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            var order = _context.Orders.Where(o => o.UserId == Userid && !o.IsFinally).Include(o => o.OrderDetail)
                .ThenInclude(p => p.Product).ThenInclude(s => s.Specifications).FirstOrDefault();

            return View(order);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        //payment
        [Authorize]
        public IActionResult Payment()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = _context.Orders.Include(o => o.OrderDetail)
                .FirstOrDefault(o => o.UserId == userId && !o.IsFinally);

            if (order == null)
            {
                return NotFound();
            }

            var payment = new Payment((int)order.OrderDetail.Sum(d => d.Price));
            var res = payment.PaymentRequest($"پرداخت فاکتور شماره {order.OrderID}",
                "http://localhost:1635/Home/OnlinePayment/" + order.OrderID, "Iman@Madaeny.com", "09197070750");
            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }
            else
            {
                return BadRequest();
            }


        }

        public IActionResult OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"].ToString();
                var order = _context.Orders.Include(o => o.OrderDetail)
                    .FirstOrDefault(o => o.OrderID == id);
                var payment = new Payment((int)order.OrderDetail.Sum(d => d.Price));
                var res = payment.Verification(authority).Result;
                if (res.Status == 100)
                {
                    order.IsFinally = true;
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                    ViewBag.code = res.RefId;
                    return View();
                }
            }

            return NotFound();
        }

    }
}


using Floristic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using NuGet.Protocol.Plugins;
using static NuGet.Packaging.PackagingConstants;

namespace Floristic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataProtector _protector;

        private readonly FloristicsContext db;
        public HomeController(ILogger<HomeController> logger, IDataProtectionProvider provider, FloristicsContext _db)
        {
            _logger = logger;
            db = _db;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Login));
            }
            var floristic = db.Florists.ToList();
            var bouquets = db.Bouquets.ToList();
            var orders = db.Orders.ToList();
            ViewBag.Florists = floristic;
            ViewBag.Bouquets = bouquets;
            return View(orders);
        }

        public IActionResult Bouquet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Login));
            }
            var bouquets = db.Bouquets.ToList();
            return View(bouquets);
        }

        public IActionResult Florist()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Login));
            }
            var floristic = db.Florists.ToList();
            var bouquets = db.Bouquets.ToList();
            var orders = db.Orders.ToList();
            return View(floristic);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrder(int bouquetId, DateTime date, DateTime time, string address, int floristId)
        {
            Console.WriteLine("AddOrder");
            Console.WriteLine(date.ToString());
            Console.WriteLine(time.ToString());

            if (ModelState.IsValid)
            {

                Order newOrder = new Order();
                Console.WriteLine("GOOD");
                newOrder.Id = db.Orders.Max(x => x.Id) + 1;
                newOrder.BouquetId = bouquetId;
                newOrder.Date = DateOnly.FromDateTime(date);
                newOrder.Time = TimeOnly.FromDateTime(time);
                newOrder.Address = address;
                newOrder.FloristId = floristId;

                    db.Orders.Add(newOrder);
                    db.SaveChanges();
                return RedirectToAction(nameof(Index));


            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> DeleteOrder(int Id)
        {
            Console.WriteLine(Id);
            if (ModelState.IsValid)
            {

                if (Id == null)
                {

                }
                else
                {
                    var orders = db.Orders.ToList();
                    if (orders.Any(s => s.Id == Id))
                    {
                        Order order = orders.Find(s => s.Id == Id);


                        db.Orders.Remove(order);
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBouquet(string Name, string? Description)
        {


            if (ModelState.IsValid)
            {

                if (Name == null || Name.Length == 0 || Description == null || Description.Length == 0)
                {

                }
                else
                {
                    Bouquet newBouquet = new Bouquet();
                    newBouquet.Id = db.Bouquets.Max(x => x.Id) + 1;
                    newBouquet.Name = Name;
                    newBouquet.Description = Description;
                    db.Bouquets.Add(newBouquet);
                    db.SaveChanges();
                }
            }

            return RedirectToAction(nameof(Bouquet));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> DeleteFlorist(int Id)
        {
            Console.WriteLine(Id);
            if (ModelState.IsValid)
            {

                if (Id == null)
                {

                }
                else
                {
                    var florists = db.Florists.ToList();
                    var orders = db.Orders.ToList();
                    if (florists.Any(s => s.Id == Id))
                    {
                        orders.ForEach(o =>
                        {
                            if (o.FloristId == Id)
                            {
                                db.Orders.Remove(o);
                            }
                        });
                        Florist florist = florists.Find(s => s.Id == Id);


                        db.Florists.Remove(florist);
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction(nameof(Florist));
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFlorist(string FullName, string ShortName)
        {


            if (ModelState.IsValid)
            {
             
                if (ShortName == null  || ShortName.Length==0 || FullName == null || FullName.Length == 0)
                {
                
                }
                else {
                    Florist newFlorist = new Florist();
                    newFlorist.Id=db.Florists.Max(x => x.Id)+1;
                    newFlorist.ShortName = ShortName;
                    newFlorist.FullName = FullName;
                    db.Florists.Add(newFlorist);
                    db.SaveChanges();
                }
            }
          
            return RedirectToAction(nameof(Florist));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> DeleteBouquet(int Id)
        {
            Console.WriteLine(Id);
            if (ModelState.IsValid)
            {

                if (Id == null)
                {

                }
                else
                {
                    var bouquets = db.Bouquets.ToList();
                    var orders = db.Orders.ToList();
                    if (bouquets.Any(s => s.Id == Id))
                    {
                        orders.ForEach(o =>
                        {
                            if (o.BouquetId == Id)
                            {
                                db.Orders.Remove(o);
                            }
                        });
                        Bouquet bouquet = bouquets.Find(s => s.Id == Id);


                        db.Bouquets.Remove(bouquet);
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction(nameof(Bouquet));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Floristic.Models;
using Floristic.Controllers;
using Microsoft.AspNetCore.DataProtection;

namespace UniversitiesMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataProtector _protector;
        public AccountController(ILogger<HomeController> logger, IDataProtectionProvider provider)
        {
            _logger = logger;
            _protector = provider.CreateProtector("Floristic.Auth.v1");
        }

        // тестовые данные вместо использования базы данных
        private List<Person> people = new List<Person>
        {
            new Person {Login="CfDJ8FVfmhPFLOdHnphU4JAkJp8KQG8w3nW2gDitn_aLKxR8ESTzGkjeBuYDtHGs8OAZOwrQ8vvKvRA0kw9Ym605tjTLPds4Spmho2VKpr4RgK4CmGSOl7AoBPG_WbJ7kJExKw", Password="CfDJ8FVfmhPFLOdHnphU4JAkJp8XgEsnOLQqldgPhdRd_VlFzaN6oO3J9SiFvCsBYXYOOR0qezztxu15oowpfVO1GeaznswpKp6ZSCD429125ZdJO7TFRMLrAeb0ncWS_kKSsg", Role = "admin" },
            new Person { Login="CfDJ8FVfmhPFLOdHnphU4JAkJp8jUDYLsdevVhi74tHcDVyM7BNlX5ZQr7Y7HiOI6tsKrzsX_G8TTLfGD6YSSC6sABxVes3YHHuQc1orYJpw1yOeiJXGNDL2P3eEyQ4qWafHbw", Password="CfDJ8FVfmhPFLOdHnphU4JAkJp88JS1sGlbwn2CpClWI32DIHlw4Yb2chbgk-PFgf7tYpJr0sKTg02najJg02L8QZLbrMgKeXZetJ8fC0yKeSelTpCI9PLnrfhTYcen1PiNctA", Role = "user" }
        };

   

        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            string protectedLogin = _protector.Protect(username);
            Console.WriteLine($"login - Protect returned: {protectedLogin}");
            string protectedPassword = _protector.Protect(password);
            Console.WriteLine($"password - Protect returned: {protectedPassword}");


            string unprotectedLogin = _protector.Unprotect(protectedLogin);
            Console.WriteLine($"login - Unprotect returned: {unprotectedLogin}");
            string unprotectedPassword = _protector.Unprotect(protectedPassword);
            Console.WriteLine($"password - Unprotect returned: {unprotectedPassword}");
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return Unauthorized(new { errorText = "Invalid username or password" });
            }

            var claimsPrincipal = new ClaimsPrincipal(identity);
            HttpContext.SignInAsync(claimsPrincipal).Wait();

            return Ok(new { redirectTo = Url.Action("Index", "Home") });
        }

        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            Console.WriteLine("logout");
            HttpContext.SignOutAsync().Wait();
            Console.WriteLine(User.Identity.IsAuthenticated);
            Console.WriteLine(User.Identity.Name);
            return Ok();
        }


        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var person = people.FirstOrDefault(x => Decrypt(x.Login) == username && Decrypt(x.Password) == password);
            //var person = people.FirstOrDefault(x => (x.Login) == username && (x.Password) == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, Decrypt(person.Login)),
                    //new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        public string Encrypt(string plaintext)
        {
            return _protector.Protect(plaintext);
        }

        public string Decrypt(string ciphertext)
        {
            return _protector.Unprotect(ciphertext);
        }
    }

}

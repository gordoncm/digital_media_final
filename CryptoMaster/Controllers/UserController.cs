using CryptoMaster.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMaster.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index(string userName)
        {
            using var dbContext = new CryptoDbContex();

            if (userName != null)
            {
                var currentUser = dbContext.Users.Where(p => p.Name == userName).FirstOrDefault();

                if (currentUser != null)
                {
                    var isLoggedIn = currentUser.IsLoggedIn;

                    if (isLoggedIn)
                    {
                        var tokens = GetPortfolioAPI.GetFromDB(userName); 

                        return View("GetPortfolio", tokens);
                    }
                }
            } 

            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult SavePortfolio(UserModel userModel)
        { 
            using var db = new CryptoDbContex();
            var tokenAddress = db.Users.Where(p => p.Name == userModel.Name).FirstOrDefault();

            var tokens = GetPortfolioAPI.GetPortfolioTokens(tokenAddress.BSCAddress, userModel.Name); 

            return View("GetPortfolio", tokens); 
        }
 

        public IActionResult Register()
        {
            return View("Register"); 
        }

        public IActionResult Login()
        {
            return View("Login"); 
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterUser(UserModel userModel)
        {
            Random random = new Random();
            int randomInt = random.Next(1, 101);

            using var dbContext = new CryptoDbContex();

            var user = dbContext.Users.Where(p => p.Name == userModel.Name).FirstOrDefault(); 

            if (user == null)
            {
                var newUser = new UserModel
                {
                    Id = randomInt,
                    Name = userModel.Name,
                    Email = userModel.Email,
                    Password = userModel.Password,
                    BSCAddress = userModel.BSCAddress,
                    IsLoggedIn = true
                };

                dbContext.Add(newUser);
                dbContext.SaveChanges();


                return RedirectToAction("Index", new { userName = newUser.Name });
            }

            return View("Register"); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginUser(UserModel userModel)
        {
            if (userModel.Name != null && userModel.Password != null)
            {
                using var db = new CryptoDbContex();

                var user = db.Users.Where(p => p.Name == userModel.Name).FirstOrDefault(); 

                if (user != null)
                {
                    if (user.Password == userModel.Password)
                    {
                        user.IsLoggedIn = true; 

                        db.Update(user);
                        db.SaveChanges();

                        return RedirectToAction("Index", new { userName = userModel.Name });
                    }
                }
            }

            return View("Login"); 
        }
    }
}
